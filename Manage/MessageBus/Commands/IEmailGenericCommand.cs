namespace MessageBus.Commands
{
    public interface IEmailGenericCommand
    {
        string CorrelationId { get; set; }

        string ToName { get; set; }

        string ToEmail { get; set; }

        string Subject { get; set; }

        string Message { get; set; }
    }
}