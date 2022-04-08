using System.Threading.Tasks;

namespace Manage.Listings.Processor.Services.Interfaces
{
    public interface IListingProcessor
    {
        Task<string> ProcessNewListings();

        Task ReceiveElasticSearchEvent(string correlationId, string listingId, string processorAction);
    }
}