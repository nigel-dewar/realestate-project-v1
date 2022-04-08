using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Search.Common.Models;
using Search.Common.Models.Requests;
using Search.Common.Models.Responses;
using Search.Elastic.Service.Services.Interfaces;


namespace Search.Elastic.Service.Services
{
    public class PostCodesService: IPostCodesService
    {

        private readonly ElasticClient _client;
        private IDistributedCache _redisCache;
        private List<PostCodeModel> _postCodes;
        private readonly IConfiguration _configuration;

        public PostCodesService(IDistributedCache redisCache, IConfiguration configuration)
        {
            _redisCache = redisCache;
            _configuration = configuration;

            var settings = new ConnectionSettings(new Uri(_configuration["ConnectionStrings:ElasticSearch"]))
                .DefaultIndex("postcodes");
            _client = new ElasticClient(settings);
            
            string postCodesRedisCache = _redisCache.GetString("postcodes");
            if (postCodesRedisCache != null)
            {
                _postCodes = JsonConvert.DeserializeObject<List<PostCodeModel>>(postCodesRedisCache);
            }
            else
            {
                SeedPostCodes().Wait();
            }

        }


        public async Task<List<PostCodeModel>> FindPostCodes(string request)
        {
            try
            {
                // string postCodesRedisCache = await _redisCache.GetStringAsync("postcodes");
                // var postCodes = JsonConvert.DeserializeObject<List<PostCodeModel>>(postCodesRedisCache);
        
                // if (!_postCodes.Any())
                // {
                //     string postCodesRedisCache = await _redisCache.GetStringAsync("postcodes");
                //     _postCodes = JsonConvert.DeserializeObject<List<PostCodeModel>>(postCodesRedisCache);
                // }

                var loweredRequest = request.ToLower();

                var res = Task.Run(() =>
                {
                    return _postCodes.Where(x =>
                        string.IsNullOrEmpty(loweredRequest) ||
                        (
                            x.PostCode.ToString().ToLower().Contains(loweredRequest) ||
                            x.Locality.ToLower().Contains(loweredRequest)
                        )
                    ).ToList();

                });

                return await res;


                //await SeedPostCodes();

                // var searchResponse = await _client.SearchAsync<PostCodeModel>();

                // var searchResponse = await _client.SearchAsync<PostCodeModel>(s => s
                //     .AllIndices()
                //     .From(0)
                //     .Size(100)
                //     .Query(q => q
                //         .Wildcard(m => m
                //             .Field(f => f.PostCode)
                //             .Value(request + "*")
                //         ) || q
                //         .QueryString(m => m
                //             .Fields(f => f)
                //             .Query(request + "*")
                //             .AnalyzeWildcard(true)
                //             .AllowLeadingWildcard(true)
                //         
                //         )
                //     )
                // );

                // var searchResponse = await _client.SearchAsync<PostCodeModel>(s => s
                //     .AllIndices()
                //     .From(0)
                //     .Size(100)
                //     .Query(q => q
                //             .Term(m => m
                //                 .Field(f => f.PostCode.ToLower())
                //                 .Value(request)
                //             ) 
                //     )
                // );

                // return searchResponse.Documents.ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void HydrateRedisFromElasticDb()
        {
            try
            {
                // Seed PostCodes if not already exists
                SeedPostCodes().Wait();
                
                var response = _client.Search<PostCodeModel>(s=> s
                    .From(0)
                    .Size(10000)
                );

                // Serialize response from ElasticSearch into Redis
                var json = JsonConvert.SerializeObject(response.Documents.ToList());
                _redisCache.SetString("postcodes", json);
                
                _postCodes = JsonConvert.DeserializeObject<List<PostCodeModel>>(json);
                // string postCodesRedisCache = _redisCache.GetString("postcodes");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

        }

        
        private async Task SeedPostCodes()
        {
            try
            {
                var searchResponse = await _client.SearchAsync<PostCodeModel>();

                var data = searchResponse.Documents;

                if (!data.Any())
                {
                    string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"australian_postcodes.csv");
                    using (var reader = new StreamReader(path))
                    {


                        List<PostCodeModel> PostCodes = new List<PostCodeModel>();

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            string x = values[0];

                            string sub = values[1].ToLower().Replace(" ", "-") + "-" + values[0];

                            string localityCapitalized = Helpers.Helpers.ToTitleCase(values[1]);

                            string label = $"{localityCapitalized}, {values[2]}, {values[0]}";

                            if (x != "postcode")
                            {
                                PostCodes.Add(new PostCodeModel
                                    {
                                        PostCode = values[0],
                                        Locality = values[1],
                                        State = values[2],
                                        Long = values[3],
                                        Lat = values[4],
                                        Id = int.Parse(values[5].ToString()),
                                        // DeliveryCentre = values[6],
                                        Type = values[7],
                                        Status = values[8],
                                        Suburb = sub,
                                        Label = label
                                    }
                                );
                            }
                        
                        }

                        var codesToAdd = PostCodes.OrderBy(x => x.PostCode).ToList();

                        // foreach (var code in codesToAdd)
                        // {
                        //     await _client.IndexDocumentAsync(code);
                        // }
                        
                        
                        var json = JsonConvert.SerializeObject(codesToAdd);
                        _redisCache.SetString("postcodes", json);
                
                        _postCodes = JsonConvert.DeserializeObject<List<PostCodeModel>>(json);
                    
                    }
                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task Purge()
        {
            await _client.DeleteByQueryAsync<PostCodeModel>(del => del
                .Query(q => q.QueryString(qs => qs.Query("*")))
            );

            await _redisCache.SetStringAsync("postcodes", "");
        }

    }
}