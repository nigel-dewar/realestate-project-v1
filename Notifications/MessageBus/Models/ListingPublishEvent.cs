using MessageBus.Events;

namespace MessageBus.Models
{
    public class ListingPublishEvent: IListingPublishEvent
    {
       public string ManageListingId { get; set; }

       public string CorrelationId { get; set; }

       public string ProcessorAction { get; set; }
    }
}