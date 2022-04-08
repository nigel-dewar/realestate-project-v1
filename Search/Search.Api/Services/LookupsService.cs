using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Constants;
using MessageBus.Events;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Search.Common.Models;
using Search.Common.Models.Requests;
using Search.Common.Models.Responses;
using Search.Elastic.Service.Services.Interfaces;
using Serilog;


namespace Search.Elastic.Service.Services
{
    public class LookupsService: ILookupsService
    {

        private readonly ElasticClient _client;
        private IDistributedCache _redisCache;
        private LookupsModel _lookups;
        private readonly IBusControl _busControl;
        private readonly IConfiguration _configuration;
        private ILogger _logger;

        public LookupsService(IDistributedCache redisCache, IBusControl busControl, ILogger logger, IConfiguration configuration)
        {
            _redisCache = redisCache;
            _busControl = busControl;
            _logger = logger;
            _configuration = configuration;

            var settings = new ConnectionSettings(new Uri(_configuration["ConnectionStrings:ElasticSearch"]))
                .DefaultIndex("lookups");
            _client = new ElasticClient(settings);
        }


        public async Task<LookupsModel> GetLookups()
        {
            try
            {
                var _lookups = new LookupsModel
                {
                    Features = new List<string> {
                        "Air Conditioning",
                        "Heating",
                        "Fire place",
                        "Fans",
                        "Evaporative cooling",
                        "Dishwasher",
                        "Garage",
                        "Remote Garage",
                        "Carport",
                        "Secure Parking",
                        "Alarm System",
                        "Intercom",
                        "Fully Fenced",
                        "Outdoor entertaining area",
                        "Swimming pool - Above ground",
                        "Swimming pool - In ground",
                        "Outside Spa",
                        "Indoor Spa",
                        "Gym",
                        "Workshop",
                        "Yard for kids to play",
                        "Balcony"
                    },
                    PropertyTypes = new List<string> {
                        "House",
                        "Apartment",
                        "Unit",
                        "Villa",
                        "Townhouse",
                        "Acreage"
                    }
                };
                //string lookupsRedisCache = await _redisCache.GetStringAsync("lookups");
                //if (lookupsRedisCache != null)
                //{
                //    _lookups = JsonConvert.DeserializeObject<LookupsModel>(lookupsRedisCache);
                //}
                //else
                //{
                //    await HydrateRedisFromElasticDb();


                //}
                return _lookups;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task HydrateRedisFromElasticDb()
        {
            try
            {

                SeedLookups().Wait();
                
                var response = _client.Search<LookupsModel>(s=> s
                    .From(0)
                    .Size(10000)
                );

                if (response.Documents.Any())
                {
                    var json = JsonConvert.SerializeObject(response.Documents.First());
                    _redisCache.SetString("lookups", json);
                
                    _lookups = JsonConvert.DeserializeObject<LookupsModel>(json);
                }

                // Serialize response from ElasticSearch into Redis
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

        }
        
        
        public async Task Purge()
        {
            try
            {
                await _client.DeleteByQueryAsync<LookupsModel>(del => del
                    .Query(q => q.QueryString(qs => qs.Query("*")))
                );

                await _redisCache.SetStringAsync("lookups", "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private async Task SeedLookups()
        {
            try
            {
                var searchResponse = await _client.SearchAsync<LookupsModel>();

                var data = searchResponse.Documents;

                if (!data.Any())
                {
                    var rabbitUri = $"rabbitmq://{_configuration["EventBus:HostName"]}/";
                    var sentToUri =
                        new Uri(rabbitUri + $"{EventBusConstants.GetLookupsFromManageApiCommandQueue}");
                    var client = _busControl.CreateRequestClient<IGetLookupsCommand>(sentToUri);

                    var response = await client.GetResponse<IGetLookupsCommandResult>(new
                        {});

                    if (!response.Message.Features.Any())
                    {
                        _logger.Error("Could not get Lookups from the Manage API Service");
                        throw new Exception("No Lookups were returned from the Manage Api Service to the Search Service");
                    }

                    var lookupsModel = new LookupsModel
                    {
                        Features = response.Message.Features,
                        PropertyTypes = response.Message.PropertyTypes
                    };
                    
                    await _client.IndexDocumentAsync(lookupsModel);
                    
                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}