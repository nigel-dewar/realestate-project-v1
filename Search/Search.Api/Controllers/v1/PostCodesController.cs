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
    public class PostCodesController : ControllerBase
    {

        private readonly IPostCodesService _postCodesService;

        public PostCodesController(IPostCodesService postCodesService)
        {
            _postCodesService = postCodesService;
        }


        [HttpPost("Search", Name = "Search")]
        public async Task<ActionResult<FindPostCodeResponse>> Search(FindPostCodeRequest request)
        {
            try
            {
                var result = await _postCodesService.FindPostCodes(request.Query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("purge")]
        public async Task<ActionResult> Purge()
        {
            try
            {

                await _postCodesService.Purge();

                return Ok("PostCodes Purged");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        

    }

}
