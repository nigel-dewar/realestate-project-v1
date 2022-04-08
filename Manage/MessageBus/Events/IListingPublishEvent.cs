namespace MessageBus.Events
{
    public interface IListingPublishEvent
    {
        string ManageListingId { get; set; }

        string CorrelationId { get; set; }

        string ProcessorAction { get; set; }

    }
}