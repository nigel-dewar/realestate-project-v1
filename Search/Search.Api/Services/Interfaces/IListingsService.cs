using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBus.Commands;
using Search.Common.Interfaces;
using Search.Common.Models;

namespace Search.Elastic.Service.Services.Interfaces
{
    public interface IListingsService: ISearchListingsInterface
    {
        Task Purge();
        
        Task<ListingModel> GetListing(string id);
        Task<ListingModel> GetListingBySlug(string slug);
        Task<ListingModel> GetListingByListingRef(string listingRef);

        Task<List<string>> GetImages(string id);

        Task SyncListing(IListingPublishCommand listingPublishCommand);
    }
}