using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using MessageBus.Commands;
using MessageBus.Events;
using Microsoft.Extensions.Configuration;
using Nest;
using Search.Common.Models;
using Search.Common.Models.Requests;
using Search.Common.Models.Responses;
using Search.Elastic.Service.Data;
using Search.Elastic.Service.Services.Interfaces;


namespace Search.Elastic.Service.Services
{
    public class ListingsService: IListingsService
    {

        private readonly ElasticClient _client;
        private readonly IConfiguration _configuration;

        public ListingsService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            var settings = new ConnectionSettings(new Uri(_configuration["ConnectionStrings:ElasticSearch"]))
                .DefaultIndex("listings");
            _client = new ElasticClient(settings);
        }

        public Task<List<ListingModel>> GetAllListings()
        {
            throw new System.NotImplementedException();
        }

        public async Task<FindListingsResponse> FindListings(FindListingMapped request)
        {

            try
            {
                // await SeedListings();

                var page = request.Page;

                var skipAmount = 0;
                if (request.Page > 1)
                {
                    skipAmount = (request.Page - 1) * 10;
                }

                var takeAmount = 10;


                var searchResponse = await _client.SearchAsync<ListingModel>(s => s
                    .From(skipAmount)
                    .Size(takeAmount)

                    .Query(q => q

                            .Range(c => c

                                .Field(p => p.Bedrooms)
                                .GreaterThanOrEquals(request.BedRooms[0])
                                .LessThanOrEquals(request.BedRooms[1])

                            ) 
                                && q.Range(c => c

                                  .Field(p => p.Bathrooms)
                                  .GreaterThanOrEquals(request.BathRooms[0])
                                  .LessThanOrEquals(request.BathRooms[1])

                              )
                              && q.Range(c => c

                                  .Field(p => p.ParkingSpaces)
                                  .GreaterThanOrEquals(request.Parking[0])
                                  .LessThanOrEquals(request.Parking[1])

                              )
                              && q.Range(c => c

                                  .Field(p => p.Price)
                                  .GreaterThanOrEquals(request.Price[0])
                                  .LessThanOrEquals(request.Price[1])

                              )
                              && q.Match(m => m
                                  .Field(f => f.SuburbSlug)
                                  .Query(request.Suburbs)
                              ) && q.Match(m => m
                                  .Field(f => f.ListingType)
                                  .Query(request.Mode)
                              ) && q.Match(m => m
                                  .Field(f => f.PropertyType)
                                  .Query(request.PropertyTypes)
                              ) && q.Match(m => m
                                  .Field(f => f.Features)
                                  .Query(request.Features)
                              )

                    ));
                

                var total = Int32.Parse(searchResponse.Total.ToString());
                
                // filter the response to keep it nice and trim
                // var listings = searchResponse.Documents.Select(x => new ListingModel
                // {
                //     Id = x.Id,
                //     Bathrooms = x.Bathrooms,
                //     Bedrooms = x.Bedrooms,
                //     ParkingSpaces = x.ParkingSpaces,
                //     Price = x.Price,
                //     SuburbSlug = x.SuburbSlug
                // }).ToList();

                var availablePages = (total / takeAmount) + 1;

                var currentQueryCount = searchResponse.Documents.Count;

                var res = new FindListingsResponse
                {
                    Properties = searchResponse.Documents.ToList(),
                    Total = total,
                    CurrentQueryCount = currentQueryCount,
                    AvailablePages = availablePages,
                    CurrentPageNumber = page
                };

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<LookupsModel> GetLookups()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<ListingModel> GetListing(string id)
        {
            try
            {
                var searchResponse = await _client.GetAsync<ListingModel>(id);

                return searchResponse.Source;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ListingModel> GetListingBySlug(string slug)
        {
            try
            {

                // var searchResponse = await _client.GetAsync<ListingModel>(slug);
                var searchResponse = await _client.SearchAsync<ListingModel>(s=> s
                    .Query(q => q
                        .Match(m => m
                            .Field(f => f.Slug)
                            .Query(slug)
                        )
                    )
                );

                return searchResponse.Documents.FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ListingModel> GetListingByListingRef(string listingRef)
        {
            try
            {
                var searchResponse = await _client.SearchAsync<ListingModel>(s=> s
                    .Query(q => q
                        .Match(m => m
                        .Field(f => f.ListingRef)
                        .Query(listingRef)
                    )
                    )
                );

                return searchResponse.Documents.FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<string>> GetImages(string id)
        {
            try
            {
                var searchResponse = await _client.GetAsync<ListingModel>(id, s => s.SourceIncludes(x=>x.Images));
                
                return searchResponse.Source.Images;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SyncListing(IListingPublishCommand command)
        {
            try
            {
                if (command.ProcessorAction == "Publish")
                {
                    await PublishOrUpdateListing(command);
                }
                
                if (command.ProcessorAction == "Update")
                {
                    await PublishOrUpdateListing(command);
                }
                
                if (command.ProcessorAction == "Relist")
                {
                    await PublishOrUpdateListing(command);
                }
                
                if (command.ProcessorAction == "Cancel")
                {
                    await RemoveListing(command);
                }
                
                if (command.ProcessorAction == "Expire")
                {
                    await RemoveListing(command);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        

        private async Task RemoveListing(IListingPublishCommand command)
        {
            try
            {
                var id = command.Slug + command.ListingRef;
                var searchResponse = await _client.GetAsync<ListingModel>(id);

                if (searchResponse.Found)
                {
                    await _client.DeleteAsync<ListingModel>(id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task PublishOrUpdateListing(IListingPublishCommand command)
        {
            try
            {
                var newlisting = new ListingModel()
                {
                    Id = command.Slug + command.ListingRef,
                    ManageListingId = command.ListingId,
                    ListingRef = command.ListingRef,
                    Title = command.Title,
                    Slug = command.Slug,
                    Address = command.Address,
                    MainImage = command.MainImage,
                    Images = command.Images,
                    Description = command.Description,
                    ListingType = command.ListingType,
                    PropertyType = command.PropertyType,
                    SuburbSlug = command.SuburbSlug,
                    LandSize = command.LandSize,
                    RegionOrState = command.RegionOrState,
                    Features = command.Features,
                    Bedrooms = command.Bedrooms,
                    Bathrooms = command.Bathrooms,
                    ParkingSpaces = command.ParkingSpaces,
                    Price = command.Price
                };
                
                // check if already exists
                var id = command.Slug + command.ListingRef;
                var searchResponse = await _client.GetAsync<ListingModel>(id);

                if (searchResponse.Found)
                {
                    // already exists so just update
                    await _client.UpdateAsync<ListingModel>(id, descriptor => descriptor.Doc(newlisting));
                }
                else
                {
                    // does not exist so create new
                    await _client.IndexDocumentAsync(newlisting);
                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task SeedListings()
        {
            await Seed.SeedData(_client);
        }
        
        public async Task Purge()
        {
            await _client.DeleteByQueryAsync<ListingModel>(del => del
                .Query(q => q.QueryString(qs => qs.Query("*")))
            );
            
        }
        
    }
    
}