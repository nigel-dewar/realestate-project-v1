namespace MessageBus.Commands
{
    public interface IEmailWelcomeCommand
    {
        string CorrelationId { get; set; }
        string ToEmail { get; set; }
    }
}