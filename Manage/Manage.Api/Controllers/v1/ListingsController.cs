using System.Collections.Generic;
using System.Threading.Tasks;
using Manage.API.Interfaces;
using Manage.API.Models.v1.Requests.Listings;
using Manage.API.Models.v1.Requests.Properties;
using Manage.API.Models.v1.Responses.Images;
using Manage.API.Models.v1.Responses.Listings;
using Manage.API.Models.v1.Responses.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manage.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ListingsController : ControllerBase
    {
        
        private readonly IListingService _listingsService;
        
        public ListingsController(IListingService listingService)
        {
            _listingsService = listingService;
        }
        
        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            return await _listingsService.GetListingsCount();
        }
        
        // Get all Listings by User
        // /api/v1/listings/lookups
        [HttpGet("lookups")]
        public async Task<ActionResult<ListingLookupsResponse>> GetLookups()
        {
            return await _listingsService.GetLookups();
        }

        // Get all Listings by User
        // /api/v1/listings
        [HttpGet()]
        public async Task<ActionResult<List<ListingWithPropertyResponse>>> GetAds()
        {
            return await _listingsService.GetListings();
        }
        
        // Get Listing by ID
        // /api/v1/listings/45646-54654-54654-465456
        [HttpGet("{id}")]
        public async Task<ActionResult<ListingWithPropertyResponse>> Get(string id)
        {
            return await _listingsService.GetListing(id);
        }
        
        // Create Listing using Property Id
        // /api/v1/listings/create-listing
        [HttpPost("create-listing")]
        public async Task<ActionResult<ListingWithPropertyResponse>> Create(CreateListingRequest request)
        {
            return await _listingsService.CreateListing(request);
        }
        
        // Create Property
        // /api/v1/listings/create-property
        [HttpPost("create-property")]
        public async Task<ActionResult<CreatePropertyResponse>> Create(CreatePropertyRequest propertyIn)
        {
            return await _listingsService.CreateProperty(propertyIn);
        }
        
        // Save or Update Listing
        // /api/v1/listings
        [HttpPost()]
        public async Task<ActionResult<ListingWithPropertyResponse>> SaveListing(SaveListingRequest request)
        {
            return await _listingsService.SaveListing(request);
        }
        
        // Get Images for Property
        // /api/v1/listings/property/45646-54654-54654-465456/images
        [HttpGet("property/{id}/images")]
        public async Task<ActionResult<List<PropertyImageResponse>>> GetImages(string id)
        {
            return await _listingsService.GetImagesForProperty(id);
        }
        
        // Add Image to Property
        // /api/v1/listings/property/45646-54654-54654-465456/addImage
        [HttpPost("property/{id}/addImage")]
        public async Task<ActionResult<CreateImageResponse>> AddImage([FromForm] IFormFile file, string id)
        {
            return await _listingsService.AddImageToProperty(file, id);
        }
        
        // Set main Image
        // /api/v1/listings/property/45646-54654-54654-465456/addImage
        [HttpPost("property/{id}/setMainImage/{imageId}")]
        public async Task<ActionResult<string>> SetMainImage(string imageId, string id)
        {
            return await _listingsService.SetMainImageForProperty(imageId, id);
        }
        
        // Delete an Image from Property
        // /api/v1/listings/property/45646-54654-54654-465456/deleteImage/5465465654
        [HttpDelete("property/{id}/deleteImage/{imageId}")]
        public async Task<ActionResult<bool>> DeleteImage(string imageId, string id)
        {
            return await _listingsService.DeleteImageFromProperty(imageId, id);
        }
        
        // Create Property
        // /api/v1/listings/create-property
        [HttpPost("can-create-address-check")]
        public async Task<ActionResult<CanCreateAddressResponse>> CanCreateAddressCheck(CanCreateAddressRequest request)
        {
            return await _listingsService.CanCreateAddressCheck(request);
        }
        
        // Create Property
        // /api/v1/listings/create-property
        [HttpGet("CanListProperties/{listingType}")]
        public async Task<ActionResult<List<CanListPropertyResponse>>> CanListProperties(string listingType)
        {
            return await _listingsService.CanListProperties(listingType);
        }
        
        
    }
}