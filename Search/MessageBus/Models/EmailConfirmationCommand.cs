using MessageBus.Commands;

namespace MessageBus.Models
{
    public class EmailConfirmationCommand: IEmailConfirmationCommand
    {
        public string CorrelationId { get; set; }
        public string ToEmail { get; set; }
        public string ConfirmEmailUrl { get; set; }
    }
}