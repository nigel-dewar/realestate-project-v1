using System.Collections.Generic;
using System.Threading.Tasks;
using Manage.API.Models.v1.Requests.Listings;
using Manage.API.Models.v1.Requests.Properties;
using Manage.API.Models.v1.Responses.Images;
using Manage.API.Models.v1.Responses.Listings;
using Manage.API.Models.v1.Responses.Properties;
using Microsoft.AspNetCore.Http;

namespace Manage.API.Interfaces
{
    public interface IListingService
    {
        Task<List<ListingWithPropertyResponse>> GetListings();
        
        Task<ListingWithPropertyResponse> GetListing(string listingId);
        Task<ListingWithPropertyResponse> CreateListing(CreateListingRequest propertyId);
        Task<CreatePropertyResponse> CreateProperty(CreatePropertyRequest request);
        
        Task<ListingWithPropertyResponse> SaveListing(SaveListingRequest request);
        
        
        Task<CreateImageResponse> AddImageToProperty(IFormFile file, string propertyId);
        
        Task<bool> DeleteImageFromProperty(string imageId, string propertyId);

        Task<List<PropertyImageResponse>> GetImagesForProperty(string propertyId);
        
        Task<string> SetMainImageForProperty(string imageId, string propertyId);

        Task<ListingLookupsResponse> GetLookups();
        Task<int> GetListingsCount();
        Task<CanCreateAddressResponse> CanCreateAddressCheck(CanCreateAddressRequest request);
        Task<List<CanListPropertyResponse>> CanListProperties(string listingType);
    }
}