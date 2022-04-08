using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Search.Common.Interfaces;
using Search.Common.Models;
using Search.Common.Models.Requests;
using Search.Common.Models.Responses;
using Search.Elastic.Service.Services;
using Search.Elastic.Service.Services.Interfaces;

namespace Search.Elastic.Service.Controllers.v1
{
    
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {

        private readonly ILookupsService _lookupsService;

        public LookupsController(ILookupsService lookupsService)
        {
            _lookupsService = lookupsService;
        }
        
        [HttpGet]
        public async Task<ActionResult<LookupsModel>> GetLookups()
        {
            try
            {

                return Ok(await _lookupsService.GetLookups());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet("purge")]
        public async Task<ActionResult> Purge()
        {
            try
            {
                await _lookupsService.Purge();
                return Ok("lookups purged");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        

    }

}
