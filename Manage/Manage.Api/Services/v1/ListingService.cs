using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Automatonymous.Activities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Manage.API.Helpers;
using Manage.API.Interfaces;
using Manage.API.Models;
using Manage.API.Models.v1.DTO;
using Manage.API.Models.v1.Requests.Listings;
using Manage.API.Models.v1.Requests.Properties;
using Manage.API.Models.v1.Responses.Images;
using Manage.API.Models.v1.Responses.Listings;
using Manage.API.Models.v1.Responses.Properties;
using Manage.Repository.Data;
using Manage.Repository.Entities;
using MessageBus.Events;
using MessageBus.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Manage.API.Services.v1
{
    public class ListingService: IListingService
    {
        private readonly ManageDataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly IImageService _imageService;
        private readonly ILogger _logger;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        

        public ListingService(ManageDataContext context, IMapper mapper, IUserAccessor userAccessor, IImageService imageService, ILogger logger, IMailService mailService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _imageService = imageService;
            _logger = logger;
            _mailService = mailService;
            _configuration = configuration;
        }


        public async Task<List<ListingWithPropertyResponse>> GetListings()
        {
            try
            {
                using (_context)
                {
                    var userId = _userAccessor.GetCurrentUserId();
                    var claims = _userAccessor.GetUserClaims();
                    
                    _logger.Information("({userId}) is about to call. {claims}", userId, claims);
                    
                    return await _context.Listings.Where(x => x.UserId == _userAccessor.GetCurrentUserId())
                        .Include(x=>x.Property)
                        .Select(x=> new ListingWithPropertyResponse
                        {
                            Id = x.Id,
                            ListingRef = x.ListingRef,
                            PropertyId = x.PropertyId,
                            DateCreated = x.DateCreated,
                            DateListingStarts = x.DateListingStarts,
                            DateListingExpires = x.DateListingExpires,
                            Active = x.Active,
                            Confirmed = x.Confirmed,
                            Status = x.Status,
                            StatusCode = x.StatusCode,
                            Complete = x.Complete,
                            Cancelled = x.Cancelled,
                            Price = x.Price,
                            ListingType = x.ListingType,
                            IsPremium = x.IsPremium,
                            IsListedByAgent = x.IsListedByAgent,
                            IsPrivateSeller = x.IsPrivateSeller,
                            AgencyId = x.AgencyId,
                            AgentId = x.AgentId,
                            Name = x.Property.Name,
                            Description = x.Property.Description,
                            Slug = x.Property.Slug,
                            SuburbSlug = x.Property.SuburbSlug,
                            PropertyTypeId = x.Property.PropertyTypeId,
                            Bedrooms = x.Property.Bedrooms,
                            Bathrooms = x.Property.Bathrooms,
                            ParkingSpaces = x.Property.ParkingSpaces,
                            LandSize = x.Property.LandSize,
                            MainImage = x.Property.MainImage,
                            PostalCode = x.Property.PostalCode,
                            StreetNumber = x.Property.StreetNumber,
                            StreetName = x.Property.StreetName,
                            SuburbOrCity = x.Property.SuburbOrCity,
                            RegionOrState = x.Property.RegionOrState,
                            Council = x.Property.Council,
                            Country = x.Property.Country,
                            CreatedById = x.Property.CreatedById
                        })
                        .ToListAsync();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ListingWithPropertyResponse> GetListing(string listingId)
        {
            try
            {
                using (_context)
                {
                    var listing = await _context.Listings.Where(x => x.UserId == _userAccessor.GetCurrentUserId())
                        .Include(x => x.Property)
                        .ThenInclude(x => x.PropertyFeatures)
                        .FirstOrDefaultAsync(x => x.Id == listingId);
                    
                    if (listing == null)
                        throw new RestException(HttpStatusCode.BadRequest, new { Errors = "No Property exists. Cannot create Listing."});
                    
                    var featuresResponse = new List<PropertyFeatureResponse>();

                    if (listing.Property.PropertyFeatures.Count > 0)
                    {
                        foreach (var feature in listing.Property.PropertyFeatures)
                        {
                            var feat = await _context.Features.Where(b => b.Id == feature.FeatureId).SingleAsync();
                            featuresResponse.Add(new PropertyFeatureResponse
                            {
                                Id = feat.Id,
                                Name = feat.Name
                            });
                        }
                    }

                    var res = MapListingWithPropertyResponse(listing, featuresResponse, null);
                    

                    try
                    {
                        
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error getting listing");
                        throw;
                    }

                    return res;

                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private static ListingWithPropertyResponse MapListingWithPropertyResponse(Listing listing, List<PropertyFeatureResponse> featuresResponse, CompletenessCheckerDto checker)
        {

            if (featuresResponse == null)
            {
                featuresResponse = new List<PropertyFeatureResponse>();
            }

            var res = new ListingWithPropertyResponse();

            res.Id = listing.Id;
            res.ListingRef = listing.ListingRef;
            res.Confirmed = listing.Confirmed;
            res.PropertyId = listing.PropertyId;
            res.DateCreated = listing.DateCreated;
            res.DateListingStarts = listing.DateListingStarts;
            res.DateListingExpires = listing.DateListingExpires;
            res.ContactEmail = listing.ContactEmail;
            res.ContactNumber = listing.ContactNumber;
            res.ShowNumber = listing.ShowNumber;
            res.Active = listing.Active;
            res.IsPublishedLive = listing.IsPublishedLive;
            res.Confirmed = listing.Confirmed;
            res.StatusCode = listing.StatusCode;
            res.Complete = listing.Complete;
            res.Cancelled = listing.Cancelled;
            res.Status = listing.Status;
            res.Price = listing.Price;
            res.ListingType = listing.ListingType;
            res.IsPremium = listing.IsPremium;
            res.IsListedByAgent = listing.IsListedByAgent;
            res.IsPrivateSeller = listing.IsPrivateSeller;
            res.AgencyId = listing.AgencyId;
            res.AgentId = listing.AgentId;
            res.Name = listing.Property.Name;
            res.CanEditAddress = listing.Property.CanEditAddress;
            res.AddressChanged = listing.Property.AddressChanged;
            res.Description = listing.Property.Description;
            res.Slug = listing.Property.Slug;
            res.SuburbSlug = listing.Property.SuburbSlug;
            res.PropertyTypeId = listing.Property.PropertyTypeId;
            res.Bedrooms = listing.Property.Bedrooms;
            res.Bathrooms = listing.Property.Bathrooms;
            res.ParkingSpaces = listing.Property.ParkingSpaces;
            res.LandSize = listing.Property.LandSize;
            res.MainImage = listing.Property.MainImage;
            res.PostalCode = listing.Property.PostalCode;
            res.StreetNumber = listing.Property.StreetNumber;
            res.StreetName = listing.Property.StreetName;
            res.SuburbOrCity = listing.Property.SuburbOrCity;
            res.RegionOrState = listing.Property.RegionOrState;
            res.Council = listing.Property.Council;
            res.Country = listing.Property.Country;
            res.GoogleAddress = listing.Property.GoogleAddress;
            res.Latitude = listing.Property.Latitude;
            res.Longitude = listing.Property.Longitude;
            res.CreatedById = listing.Property.CreatedById;
            res.Features = featuresResponse;
            res.Checker = checker;
            
            return res;
        }

        public async Task<ListingWithPropertyResponse> CreateListing(CreateListingRequest request)
        {
            try
            {
                using (_context)
                {
                    
                    // check if property exists and pull back any listings for it as well
                    var property = await _context.Properties.Include(x=>x.Listings).SingleOrDefaultAsync(x=>x.Id == request.PropertyId);
                    
                    if (property == null)
                        throw new RestException(HttpStatusCode.BadRequest, new { Errors = "No Property exists. Cannot create Listing."});
                    
                    var userId = _userAccessor.GetCurrentUserId();
                    
                    var codeGen = new CodeGenerator();
                    var listingRef = codeGen.RandomString(6);

                    var dateNow = DateTimeOffset.UtcNow;

                    var listing = new Listing
                    {
                        PropertyId = property.Id,
                        UserId = userId,
                        Id = Guid.NewGuid().ToString(),
                        ListingRef = listingRef,
                        Price = request.Price,
                        ListingType = request.ListingType,
                        ContactEmail = request.ContactEmail,
                        ContactNumber = request.ContactNumber,
                        ShowNumber = request.ShowNumber,
                        DateCreated = dateNow,
                        StatusCode = StatusCode.Incomplete,
                        Status = "Incomplete",
                        Active = false,
                        Complete = false
                    };

                    var offsetDate = DateTimeOffset.FromUnixTimeMilliseconds(request.DateListingStarts).UtcDateTime;
                    listing.DateListingStarts = offsetDate;

                    listing.DateListingExpires = offsetDate.AddDays(90);

                    //if (request.DateListingStarts != null)
                    //{
                    //    // var offsetDate = DateTimeOffset.Parse(request.DateListingStarts);
                    //    var offsetDate = DateTimeOffset.FromUnixTimeMilliseconds(request.DateListingStarts).UtcDateTime;
                    //    listing.DateListingStarts = offsetDate;
                    
                    //    listing.DateListingExpires = offsetDate.AddDays(90);
                    //}
                    
                    _context.Listings.Add(listing);

                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        var newListing = await _context.Listings
                            .Include(x => x.Property)
                            .Where(x => x.Id == listing.Id)
                            .SingleAsync();

                        var res = MapListingWithPropertyResponse(newListing, null, null);
                        
                        return res;

                    }

                    throw new RestException(HttpStatusCode.BadRequest, new { Errors = "Could not create listing in database"});

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<CreatePropertyResponse> CreateProperty(CreatePropertyRequest request)
        {

            try
            {
                using (_context)
                {
                    string slug = String.Concat(
                            request.StreetNumber, " ",
                            request.StreetName, " ",
                            request.SuburbOrCity, " ",
                            request.PostalCode, " ",
                            request.ListingType
                    ).ToLower().Replace(" ", "-");
                    
                    var checkPropExists = await _context.Properties.Where(x => x.Slug == slug).ToListAsync();
                    if (checkPropExists.Count > 0)
                    {
                        throw new RestException(HttpStatusCode.Conflict, new { Errors = "This property already exists"} );
                    }
                    
                    string suburbSlug = String.Concat(request.SuburbOrCity.ToLower().Replace(" ", "-"), "-", request.PostalCode.ToLower().Replace(" ", "-"));

                    var userId = _userAccessor.GetCurrentUserId();
                    
                    var name = String.Concat(request.StreetNumber, " ", request.StreetName, ", ", request.SuburbOrCity, ", ", request.PostalCode.ToLower(), " - For ", request.ListingType);

                    var property = new Property
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = name,
                        Slug = slug,
                        SuburbSlug = suburbSlug,
                        PropertyTypeId = request.PropertyTypeId,
                        Bedrooms = request.Bedrooms,
                        Bathrooms = request.Bathrooms,
                        ParkingSpaces = request.ParkingSpaces,
                        LandSize = request.LandSize,
                        PostalCode = request.PostalCode,
                        StreetNumber = request.StreetNumber,
                        StreetName = request.StreetName,
                        SuburbOrCity = request.SuburbOrCity,
                        RegionOrState = request.RegionOrState,
                        Council = request.Council,
                        Country = request.Country,
                        GoogleAddress = request.GoogleAddress,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        Address = $"{request.StreetNumber} {request.StreetName}, {request.SuburbOrCity}",
                        CreatedById = userId
                    };

                    if (request.ListingType == "Sale")
                    {
                        property.IsForSale = true;
                    }
                    else
                    {
                        property.IsForRent = true;
                    }

                    _context.Properties.Add(property);
                    
                    
                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        return new CreatePropertyResponse
                        {
                            Id = property.Id,
                            CreatedById = property.CreatedById,
                            Created = property.Created,
                            Name = property.Name,
                            AddressChanged = property.AddressChanged,
                            CanEditAddress = property.CanEditAddress,
                            Slug = property.Slug,
                            SuburbSlug = property.SuburbSlug,
                            PropertyTypeId = property.PropertyTypeId,
                            Description = property.Description,
                            Bedrooms = property.Bedrooms,
                            Bathrooms = property.Bathrooms,
                            ParkingSpaces = property.ParkingSpaces,
                            LandSize = property.LandSize,
                            StreetNumber = property.StreetNumber,
                            StreetName = property.StreetName,
                            PostalCode = property.PostalCode,
                            SuburbOrCity = property.SuburbOrCity,
                            Council = property.Council,
                            RegionOrState = property.RegionOrState,
                            Country = property.Country,
                            GoogleAddress = property.GoogleAddress,
                            Latitude = property.Latitude,
                            Longitude = property.Longitude,
                        };
                    }
                    else
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Errors = "This property could not be saved in database"} );
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
                
        }

        public async Task<ListingWithPropertyResponse> SaveListing(SaveListingRequest request)
        {

            try
            {

                var listing = await _context.Listings.Include(x=>x.ListingActions).SingleOrDefaultAsync(x => x.Id == request.Id);
                
                if (listing == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Errors = "No Listing exists for this id"});
                
                var property = await _context.Properties.Include(x=>x.PropertyFeatures).FirstOrDefaultAsync(x => x.Id == request.PropertyId);
                
                if (property == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Errors = "No Property exists for this id"});
                
                var startdate = DateTimeOffset.FromUnixTimeMilliseconds(request.DateListingStarts).UtcDateTime;
                
                listing.DateListingStarts = startdate;
                listing.DateListingExpires = startdate.AddDays(90);

                listing.Price = request.Price;
                listing.ListingType = request.ListingType;
                listing.IsPremium = request.IsPremium;
                listing.ContactEmail = request.ContactEmail;
                listing.ContactNumber = request.ContactNumber;
                listing.ShowNumber = request.ShowNumber;
                
                

                string slug = String.Concat(
                    request.StreetNumber, " ",
                    request.StreetName, " ",
                    request.SuburbOrCity, " ",
                    request.PostalCode, " ",
                    request.ListingType
                ).ToLower().Replace(" ", "-");
                
                // check to see if address has changed
                if (property.Slug != slug)
                {
                    // perform additional checks to see if user has changed the address more than once.
                    // we want to block this so dodgy users dont sit there constantly changing the address
                    if (property.AddressChanged >= 2)
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Errors = "Address has already been changed more than once. A new listing must be created to use this property address", Code = "13"} );
                    }

                    var checkPropExists = await _context.Properties.Where(x => x.Slug == slug).ToListAsync();
                    if (checkPropExists.Count > 0)
                    {
                        throw new RestException(HttpStatusCode.BadRequest, new { Errors = "This property already exists and cannot be saved", Code = "12"} );
                    }
                    
                    string suburbSlug = String.Concat(request.SuburbOrCity.ToLower().Replace(" ", "-"), "-", request.PostalCode.ToLower().Replace(" ", "-"));
                    
                    var name = String.Concat(request.StreetNumber, " ", request.StreetName, ", ", request.SuburbOrCity, ", ", request.PostalCode.ToLower(), " - For ", request.ListingType);
                    
                    // Assign new address values
                    property.Slug = slug;
                    property.SuburbSlug = suburbSlug;
                    property.Name = name;
                    property.StreetNumber = request.StreetNumber;
                    property.StreetName = request.StreetName;
                    property.PostalCode = request.PostalCode;
                    property.SuburbOrCity = request.SuburbOrCity;
                    property.Council = request.Council;
                    property.RegionOrState = request.RegionOrState;
                    property.Country = request.Country;

                    // Set limits on how many address changes are allowed
                    property.AddressChanged++;

                    if (property.AddressChanged == 1)
                    {
                        // Address has been changed once so cannot be changed again
                        property.CanEditAddress = false;
                    }

                    property.GoogleAddress = request.GoogleAddress;
                    property.Latitude = request.Latitude;
                    property.Longitude = request.Longitude;

                    // We will not allow them to change country - that means they are simply creating another property
                    //property.Country = request.Property.Country;
                }
                
                if (property.PropertyTypeId != request.PropertyTypeId)
                    property.PropertyTypeId = request.PropertyTypeId;
                
                if (property.Bedrooms != request.Bedrooms)
                    property.Bedrooms = request.Bedrooms;
                
                if (property.Bathrooms != request.Bathrooms)
                    property.Bathrooms = request.Bathrooms;
                
                if (property.ParkingSpaces != request.ParkingSpaces)
                    property.ParkingSpaces = request.ParkingSpaces;
                
                if (property.LandSize != request.LandSize)
                    property.LandSize = request.LandSize;
                
                if (property.Description != request.Description)
                    property.Description = request.Description;

                var featuresReponse = new List<PropertyFeatureResponse>();
                
               
                // Remove all the existing property features
                foreach (var propertyFeature in property.PropertyFeatures.ToList())
                {
                    
                    property.PropertyFeatures.Remove(propertyFeature);
                }

                foreach (var newFeature in request.Features)
                {
                    var feat = await _context.Features.Where(b => b.Id == newFeature.Id).SingleAsync();
                    property.PropertyFeatures.Add(new PropertyFeature { Feature = feat});
                    featuresReponse.Add(new PropertyFeatureResponse
                    {
                        Id = feat.Id,
                        Name = feat.Name
                    });
                }
                
                
                var checker = CompletenessChecker(request, listing, property);
                var complete = checker.Complete();

                // This tracks if the listing is currently within its start and end date. 
                var editable = (DateTimeOffset.Now.UtcDateTime.Date <= listing.DateListingExpires.UtcDateTime.Date) ? true : false;

                // set listing processor to nothing. If there is something to do, it will be updated
                listing.ProcessorAction = null;
                
                if (complete == true)
                {
                    if (editable)
                    {
                        if (request.UserRequestAction == "confirm")
                        {
                            // listing.Status = StatusCode.Active; // this action will be set by the cron job runner
                            listing.Confirmed = true;
                            listing.ConfirmedDateTime = DateTimeOffset.UtcNow;
                            // listing.StatusCode = StatusCode.Complete;
                            listing.Status = "Confirmed";

                            if (listing.DateListingStarts.UtcDateTime.Date == DateTimeOffset.Now.Date)
                            {
                                // TODO: MAYBE PASS OFF TO MESSAGE BUS TO PROCESS INSTANTLY
                            }
                        
                            // listing.ReadyForPublishing = true;

                            listing.ProcessorAction = "Publish";
                            listing.Active = true;

                        }
                        else
                        {
                            // listing.StatusCode = StatusCode.Complete;
                            listing.Status = "Complete";
                            
                            listing.ProcessorAction = "Update";

                        }
                    }
                }
                else
                {
                    // Do Nothing here
                    listing.Status = "Incomplete";
                }
                
                listing.Complete = complete;
                

                // if user cancels
                if (request.UserRequestAction == "cancel")
                {
                    // listing.StatusCode = StatusCode.Cancelled;
                    listing.Cancelled = true;
                    listing.Status = "Cancelled";
                    listing.IsPublishedLive = false;

                    listing.ProcessorAction = "Cancel";
                    // Cron will set this active state
                    listing.Active = false;
                }
                
                if (request.UserRequestAction == "relist")
                {
                    // listing.StatusCode = StatusCode.Cancelled;
                    listing.Cancelled = false;
                    // Cron will set this active state
                    listing.Active = false;
                    listing.Confirmed = false;
                    listing.Relisted = true;
                    listing.RelistCount++;
                    listing.Status = "Relisted";
                    
                    listing.ProcessorAction = "Relist";

                }
                
                // Runs last
                if (DateTimeOffset.UtcNow > listing.DateListingExpires)
                {
                    // listing.StatusCode = StatusCode.Expired;
                    listing.Active = false;
                    listing.Status = "Expired";
                    listing.IsPublishedLive = false;
                    
                    listing.ProcessorAction = "Expire";

                }

                if (listing.ProcessorAction != null)
                {
                    if (listing.ListingActions.Any())
                    {
                        if (listing.ListingActions.LastOrDefault().Action != listing.ProcessorAction)
                        {
                            listing.ListingActions.Add(new ListingAction()
                            {
                                Action = listing.ProcessorAction
                            });
                        }
                    }
                    else
                    {
                        listing.ListingActions.Add(new ListingAction()
                        {
                            Action = listing.ProcessorAction
                        });
                    }

                }

                // If newly published, send an email thanking user
                if (!listing.FirstEmailSent)
                {
                    try
                    {

                        string message =
                            $"Congrats. Your new Ad Reference {listing.ListingRef} {listing.Property.Name} has been successfully created. " +
                            $"<br>" +
                            $"Please find the link to administer your Ad <a href='{_configuration["AppSettings:FrontEndUrl"]}/manage/ads/{listing.Id}/manage'>here</a>.";
                        

                        var emailCommand = new EmailGenericCommand
                        {
                            Subject = $"Ad Reference: {listing.ListingRef} for '{listing.Property.Name}' has been successfully created.",
                            CorrelationId = Guid.NewGuid().ToString(),
                            ToName = request.ContactEmail,
                            ToEmail = request.ContactEmail,
                            Message = message
                        };
                        
                        await _mailService.EmailMessage(emailCommand);

                        listing.FirstEmailSent = true;
                    }
                    catch (Exception e)
                    {
                        // TODO - Dont throw error here but just Log maybe
                        Console.WriteLine(e);
                        throw;
                    }
                }
                
                // // Set Listings Processor Instructions
                //
                // if (listing.Confirmed)
                //     

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new RestException(HttpStatusCode.BadRequest, new { Errors = "Unable to save listing in database"} );
                }

                var newListing = await _context.Listings
                    .Include(x => x.Property)
                    .Where(x => x.Id == listing.Id)
                    .SingleAsync();
                
                var res = MapListingWithPropertyResponse(newListing, featuresReponse, checker);

                return res;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private CompletenessCheckerDto CompletenessChecker(SaveListingRequest request, Listing listing, Property property)
        {
            
            CompletenessCheckerDto checker = new CompletenessCheckerDto();
            // Complex function that works out if a Listing is indeed complete or not

            if (String.IsNullOrWhiteSpace(request.Description))
            {
                checker.ErrorCount++;
                checker.ErrorMessages.Add(new ErrorDto
                {
                    Code = 1,
                    Item = "description",
                    Error = "Description is required"
                });
            }

            if (request.Price <= 0)
            {
                checker.ErrorCount++;
                checker.ErrorMessages.Add(new ErrorDto
                {
                    Code = 2,
                    Item = "price",
                    Error = "Price cannot be less than or equal to Zero"
                });
            }

            if (String.IsNullOrWhiteSpace(property.MainImage))
            {
                checker.ErrorCount++;
                checker.ErrorMessages.Add(new ErrorDto
                {
                    Code = 3,
                    Item = "images",
                    Error = "At least one Picture for your Property is required"
                });

            }

            if (String.IsNullOrWhiteSpace(request.StreetNumber)) 
            {
                checker.ErrorCount++;
                checker.ErrorMessages.Add(new ErrorDto
                {
                    Code = 4,
                    Item = "streetNumber",
                    Error = "Street Number is required"
                });
     
            }

            if (String.IsNullOrWhiteSpace(request.StreetName)) 
            {
                checker.ErrorCount++;
                checker.ErrorMessages.Add(new ErrorDto
                {
                    Code = 5,
                    Item = "streetName",
                    Error = "Street Name is required"
                });
            }

            if (String.IsNullOrWhiteSpace(request.PostalCode)) 
            {
                checker.ErrorCount++;
                checker.ErrorMessages.Add(new ErrorDto
                {
                    Code = 6,
                    Item = "postalCode",
                    Error = "Postal Code cannot be empty"
                });
            }

            if (String.IsNullOrWhiteSpace(request.Country)) 
            {
                checker.ErrorCount++;
                checker.ErrorMessages.Add(new ErrorDto
                {
                    Code = 7,
                    Item = "country",
                    Error = "Country cannot be empty"
                });
            }

            // otherwise return true
            return checker;
        }

        public async Task SendPublishedConfirmationEmail(PublishedEmailDto dto)
        {
            // await _mailService.SendListingConfirmationEmail(dto.EmailAddress,
            //     $"Estateico: Congrats! Your new Ad Ref: {dto.AdRef} for {dto.PropertyName} , for has been listed successfully.",
            //     $"You can visit your ad here at {dto.Url}");
            
        }
        
        
        public async Task<List<PropertyImageResponse>> GetImagesForProperty(string propertyId)
        {
            try
            {
                var property = await _context.Properties.Include(x=>x.Images).SingleOrDefaultAsync(x => x.Id == propertyId);
            
                if (property == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Errors = "No Property exists for this id"});

                var images = new List<PropertyImageResponse>();

                if (property.Images.Any())
                {
                    foreach (var image in property.Images)
                    {
                        images.Add(new PropertyImageResponse
                        {
                            Id = image.Id,
                            Url = image.Url,
                            IsMain = image.IsMain
                        });
                    }
                }

                return images;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> SetMainImageForProperty(string imageId, string propertyId)
        {
            try
            {
                var property = await _context.Properties.Include(x=>x.Images).SingleOrDefaultAsync(x => x.Id == propertyId);
            
                if (property == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Errors = "No Property exists for this property id"});
            
                var image = property.Images.FirstOrDefault(x => x.Id == imageId);

                if (image == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Errors = "No Image exists for this image id"});

                var currentMain = property.Images.FirstOrDefault(x => x.IsMain);
                if (currentMain != null)
                    currentMain.IsMain = false;

                image.IsMain = true;

                property.MainImage = image.Url;
                
                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    return image.Url;
                }
    
                throw new RestException(HttpStatusCode.NotFound, new { Errors = "Could not set main image for property"});

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CreateImageResponse> AddImageToProperty(IFormFile file, string propertyId)
        {
            try
            {
                var property = await _context.Properties.Include(x=>x.Images).SingleOrDefaultAsync(x => x.Id == propertyId);
            
                if (property == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Errors = "No Property exists for this id"});
            
                var imageUploadResult = _imageService.AddImage(file);
            
                var image = new Image
                {
                    Url = imageUploadResult.Url,
                    Id = imageUploadResult.PublicId
                };

                if (!property.Images.Any(x => x.IsMain))
                {
                    image.IsMain = true;
                    property.MainImage = image.Url;
                }
            
                property.Images.Add(image);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    return new CreateImageResponse
                    {
                        Id = image.Id,
                        Url = image.Url,
                        IsMain = image.IsMain
                    };
                }

                throw new RestException(HttpStatusCode.NotFound, new { Errors = "Could not add image to property in database"});
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<bool> DeleteImageFromProperty(string imageId, string propertyId)
        {
            try
            {
                var property = await _context.Properties.Include(x=>x.Images).SingleOrDefaultAsync(x => x.Id == propertyId);
            
                if (property == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Errors = "No Property exists for this id"});

                var image = property.Images.FirstOrDefault(x => x.Id == imageId);
            
                if (image == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Errors = "No Image exists for this id"});
            
                if (image.IsMain)
                    throw new RestException(HttpStatusCode.BadRequest, new { Errors = "You cannot delete your main image"});

                var result = _imageService.DeleteImage(image.Id);
            
                if (result == null)
                    throw new Exception("Problem deleting image from property");

                property.Images.Remove(image);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    return true;
                }

                throw new RestException(HttpStatusCode.BadRequest, new { Errors = "Could not delete image from property in database"});
            
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<ListingLookupsResponse> GetLookups()
        {
            try
            {
                using (_context)
                {
                    var propertyTypes = await _context.PropertyTypes
                        .ToListAsync();

                    var features = await _context.Features
                        .ToListAsync();

                    return new ListingLookupsResponse
                    {
                        PropertyTypes = propertyTypes,
                        Features = features
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> GetListingsCount()
        {
            try
            {
                var userId = _userAccessor.GetCurrentUserId();
                var listingsCount = await _context.Listings.Where(x => x.UserId == userId).CountAsync();
                return listingsCount;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CanCreateAddressResponse> CanCreateAddressCheck(CanCreateAddressRequest request)
        {
            try
            {
                
                var userId = _userAccessor.GetCurrentUserId();
                
                var propertyHasListing = false;
                var listingActive = false;
                var createdByUser = false;

                var canCreate = false;
                
                string slug = String.Concat(
                    request.StreetNumber, " ",
                    request.StreetName, " ",
                    request.SuburbOrCity, " ",
                    request.PostalCode, " ",
                    request.ListingType
                ).ToLower().Replace(" ", "-");
                
                // There should only be one property per Google Exact Address
                var property = await _context.Properties.Where(x => x.Slug == slug && x.IsActive == true)
                    .SingleOrDefaultAsync();

                // If Have Property, check to see if Property Found already has an Ad attached
                if (property != null)
                {
                    // check to see if property was created by current user or by another user
                    if (property.CreatedById == userId)
                    {
                        createdByUser = true;
                    }

                    // check to see if there is a listing
                    var listing = await _context.Listings.Where(x => x.PropertyId == property.Id).SingleOrDefaultAsync();

                    if (listing != null)
                    {
                        // Listing is found
                        propertyHasListing = true;
                        
                        // Check to see if listing is InActive
                        if (listing.Active)
                        {
                            listingActive = true;
                        }
                    }

                }
                else
                {
                    // No Property Found at all which means user is good to create this property in the system
                    canCreate = true;
                }

                return new CanCreateAddressResponse
                {
                    CanCreate = canCreate,
                    Message = "",
                    CreatedByUser = createdByUser,
                    HasListing = propertyHasListing,
                    IsActiveListing = listingActive
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<CanListPropertyResponse>> CanListProperties(string listingType)
        {
            try
            {
                var userId = _userAccessor.GetCurrentUserId();

                // Select ALL Properties with Associated listings - Created by User
                var properties = await _context.Properties.Include(x=>x.Listings).Where(x=> x.CreatedById == userId)
                    .ToListAsync();

                var toReturn = new List<CanListPropertyResponse>();
                // work out what can be Relisted or What can be Listed Fresh

                // check to see we actually have some properties. Should not need to here but to anyway
                if (properties.Count > 0)
                {
                    // iterate over listings - extract out those that can be created fresh and those that can be re-listed

                    // var equalToListingType = listings.Any(x => x.ListingType == listingType && x.Active == false);
                    // var notEqualToListingType = listings.Any(x => x.ListingType != listingType && x.Active == false);
                    var propertiesCanListFresh = new List<Property>();
                    var propertiesCanRelist = new List<Property>();
                    
                    foreach (var property in properties)
                    {
                        propertiesCanListFresh =
                            property.Listings.Where(x => x.Active == false && x.ListingType != listingType).Select(x=>x.Property).ToList();
                        
                        propertiesCanRelist =
                            property.Listings.Where(x => x.Active == false && x.ListingType == listingType).Select(x=>x.Property).ToList();
                    }
                    
                    foreach (var prop in propertiesCanListFresh)
                    {
                        toReturn.Add(new CanListPropertyResponse
                        {
                            Id = prop.Id,
                            Name = prop.Name,
                            ReList = false
                        });
                    }
                    
                    foreach (var prop in propertiesCanRelist)
                    {
                        toReturn.Add(new CanListPropertyResponse
                        {
                            Id = prop.Id,
                            Name = prop.Name,
                            ReList = true
                        });
                    }
  
                }
                
                return toReturn;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}