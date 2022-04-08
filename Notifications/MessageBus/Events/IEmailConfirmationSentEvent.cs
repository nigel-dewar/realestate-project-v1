namespace MessageBus.Events
{
    public interface IEmailConfirmationSentEvent
    {
        string CorrelationId { get; set; }
        string ToEmail { get; set; }
    }
}