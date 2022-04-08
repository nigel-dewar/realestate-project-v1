using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ListingsController : ControllerBase
    {

        private readonly IListingsService _listingsService;

        public ListingsController(IListingsService listingsService)
        {
            _listingsService = listingsService;
        }

        [HttpPost("Find")]
        public async Task<ActionResult<FindListingsResponse>> Find(FindListingRequest request)
        {
        
            
            var price = request.Price.Split(',').Select(Int32.Parse).ToList();
            var bedRooms = request.BedRooms.Split(',').Select(Int32.Parse).ToList();
            var bathRooms = request.BathRooms.Split(',').Select(Int32.Parse).ToList();
            var parking = request.Parking.Split(',').Select(Int32.Parse).ToList();
        
            var mappedRequest = new FindListingMapped
            {
                Mode = request.Mode,
                Page = request.Page,
                Suburbs = request.Suburbs,
                PropertyTypes = request.PropertyTypes,
                Features = request.Features,
                Price = price,
                BedRooms = bedRooms,
                BathRooms = bathRooms,
                Parking = parking
            };
        
            var res = await _listingsService.FindListings(mappedRequest);
        
            return Ok(res);
        
            
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ListingModel>> GetById(string id)
        {
            var res = await _listingsService.GetListing(id);
        
            return Ok(res);
        }
        
        [HttpGet("images/{id}")]
        public async Task<ActionResult<List<string>>> GetImages(string id)
        {
            var res = await _listingsService.GetImages(id);
        
            return Ok(res);
        }
        
        [HttpGet("GetBySlug/{slug}")]
        public async Task<ActionResult<ListingModel>> GetBySlug(string slug)
        {
            var res = await _listingsService.GetListingBySlug(slug);
        
            return Ok(res);
        }
        
        [HttpGet("GetByListingRef{listingRef}")]
        public async Task<ActionResult<ListingModel>> GetByListingRef(string listingRef)
        {
            var res = await _listingsService.GetListingByListingRef(listingRef);
        
            return Ok(res);
        }
        
        [HttpGet("purge")]
        public async Task<ActionResult> Purge()
        {
            try
            {

                await _listingsService.Purge();

                return Ok("Listings Purged");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        
        
        
    }

}
