
using Search.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Search.Common.Models.Requests;
using Search.Common.Models.Responses;

namespace Search.Common.Interfaces
{
    public interface ISearchListingsInterface
    {
        Task<List<ListingModel>> GetAllListings();
        
        Task<FindListingsResponse> FindListings(FindListingMapped request);
        
    }
}
