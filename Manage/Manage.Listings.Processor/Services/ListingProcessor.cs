using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage.Listings.Processor.Services.Interfaces;
using Manage.Repository.Data;
using Manage.Repository.Entities;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Constants;
using MessageBus.Events;
using MessageBus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Manage.Listings.Processor.Services
{
    public class ListingProcessor: IListingProcessor
    {

        private readonly ManageDataContext _manageContext;
        private readonly IBusControl _busControl;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ListingProcessor(ManageDataContext manageContext, IBusControl busControl, ILogger logger, IConfiguration configuration)
        {
            _manageContext = manageContext;
            _busControl = busControl;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> ProcessNewListings()
        {

            try
            {
                var correlationId = Guid.NewGuid();
                // get Listings from manage api that are set to be published but not yet published
                var listings = await _manageContext.Listings
                    .Include(x=>x.ListingActions)
                    .Include(x => x.Property)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.Property)
                    .ThenInclude(x => x.PropertyFeatures)
                    .ThenInclude(x => x.Feature)
                    .Include(x => x.Property.PropertyType)
                    .Where(x => x.ProcessorAction != null)
                    .ToListAsync();

                foreach (var listing in listings)
                {
                    
                    try
                    {
                        await SendElasticSearchCommand(correlationId, listing);
                        
                        // await SendRethinkCommand(correlationId, listing);
                        
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"Could not Process Listing and Send it out to the Consumers from the Manage.Listings.Processer");
                        throw;
                    }
                    
                    // TODO: Wire up a Reciever for the Events that come back from these commands. Once receipt is confirmed, we can send off email. Then we can set to Active.
                    //listing.Active = true;


                    await _manageContext.SaveChangesAsync();

                }

                return correlationId.ToString();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task SendElasticSearchCommand(Guid correlationId, Listing listing)
        {
            try
            {
                var rabbitUri = $"rabbitmq://{_configuration["EventBus:HostName"]}/";
                var sentToUri =
                    new Uri(rabbitUri + $"{EventBusConstants.PublishListingToElasticCommandQueue}");

                var endPoint = await _busControl.GetSendEndpoint(sentToUri);

                var images = new List<string>();

                var property = listing.Property;

                foreach (var image in listing.Property.Images)
                {
                    images.Add(image.Url);
                }
                
                var features = new List<string>();
                
                foreach (var feat in listing.Property.PropertyFeatures)
                {
                    features.Add(feat.Feature.Name);
                }
                
            
                await endPoint.Send<IListingPublishCommand>(
                    new ListingPublishCommand
                    {
                        RequestId = correlationId,
                        ProcessorAction = listing.ProcessorAction,
                        ListingId = listing.Id,
                        Title = listing.Title,
                        ListingRef = listing.ListingRef,
                        PropertyId = listing.PropertyId,
                        DateListingStarts = listing.DateListingStarts.ToString(),
                        DateListingExpires = listing.DateListingExpires.ToString(),
                        ContactEmail = listing.ContactEmail,
                        ContactNumber = listing.ContactNumber,
                        MainImage = listing.Property.MainImage,
                        Images = images,
                        Slug = listing.Property.Slug,
                        SuburbSlug = listing.Property.SuburbSlug,
                        PropertyTypeId = listing.Property.PropertyTypeId,
                        PropertyType = listing.Property.PropertyType.Type,
                        Bedrooms = listing.Property.Bedrooms,
                        Bathrooms = listing.Property.Bathrooms,
                        ParkingSpaces = listing.Property.ParkingSpaces,
                        LandSize = listing.Property.LandSize,
                        PostalCode = listing.Property.PostalCode,
                        StreetNumber = listing.Property.StreetNumber,
                        StreetName = listing.Property.StreetName,
                        SuburbOrCity = listing.Property.SuburbOrCity,
                        RegionOrState = listing.Property.RegionOrState,
                        Council = listing.Property.Council,
                        Country = listing.Property.Country,
                        GoogleAddress = listing.Property.GoogleAddress,
                        Latitude = listing.Property.Latitude,
                        Longitude = listing.Property.Longitude,
                        Address = listing.Property.Address,
                        CreatedById = listing.Property.CreatedById,
                        Price = Decimal.ToInt32(listing.Price),
                        Features = features,
                        Description = listing.Property.Description,
                        IsPremium = false,
                        DateCreated = listing.DateCreated.ToString(),
                        ListingType = listing.ListingType
                    }
                );
            
                _logger.Information($"Listing Processor message sent to Mysql Service with Correlation {correlationId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task ReceiveElasticSearchEvent(string correlationId, string listingId, string processorAction)
        {
            try
            {
                var listing = await _manageContext.Listings.Include(x =>x.Property)
                    .Include(x=>x.ListingActions)
                    .Where(x => x.Id == listingId)
                    .FirstOrDefaultAsync();

                if (listing != null)
                {

                    if (listing.ProcessorAction == "Publish")
                    {
                        string message =
                            $"Congrats. Your new Ad Reference {listing.ListingRef} - {listing.Property.Name} has been successfully published and is now Live." +
                            $"<br>" +
                            $"View your Ad <a href='{_configuration["AppSettings:FrontEndUrl"]}/details/{listing.Property.Slug}{listing.ListingRef}'>{listing.Property.Slug}</a>.";
                    
                        var emailCommand = new EmailGenericCommand
                        {
                            Subject = $"Ad Reference: {listing.ListingRef} for '{listing.Property.Name}' is live.",
                            CorrelationId = Guid.NewGuid().ToString(),
                            ToName = listing.ContactEmail,
                            ToEmail = listing.ContactEmail,
                            Message = message
                        };
                
                        var rabbitUri = $"rabbitmq://{_configuration["EventBus:HostName"]}/";
                        var sentToUri = new Uri(rabbitUri + $"{EventBusConstants.EmailGenericCommand}");

                        var endPoint = await _busControl.GetSendEndpoint(sentToUri);
                        await endPoint.Send<IEmailGenericCommand>(
                            emailCommand
                        );

                        listing.IsPublishedLive = true;
                    }
                    else
                    {
                        listing.IsPublishedLive = false;
                    }

                    listing.ProcessorAction = null;
                    var action = listing.ListingActions.LastOrDefault();

                    if (action != null)
                    {
                        action.DateFulfilled = DateTimeOffset.Now;
                        action.FulfillmentId = correlationId;
                    }

                    _manageContext.Update(listing);
                    await _manageContext.SaveChangesAsync();
                    
                    
                }
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task SendRethinkCommand(Guid correlationId, Listing listing)
        {
            var sentToUri =
                new Uri($"{EventBusConstants.RabbitMqUri}" + $"{EventBusConstants.PublishListingToRethinkCommandQueue}");

            var endPoint = await _busControl.GetSendEndpoint(sentToUri);
            await endPoint.Send<IListingPublishEvent>(
                new
                {
                    CorrelationId = correlationId,
                    Id = listing.Id
                }
            );
            
            _logger.Information($"Listing Processor message sent to RethinkDb Service with Correlation {correlationId}");
        }
    }
    
}