namespace MessageBus.Commands
{
    public interface IEmailConfirmationCommand
    {
        string CorrelationId { get; set; }
        string ToEmail { get; set; }
        
        string ConfirmEmailUrl { get; set; }
    }
}