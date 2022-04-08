using MessageBus.Commands;

namespace Identity.API.Models
{
    public class EmailGenericCommand: IEmailGenericCommand
    {
        public string CorrelationId { get; set; }
        
        public string ToName { get; set; }
        
        public string ToEmail { get; set; }
        
        public string Subject { get; set; }

        public string Message { get; set; }
    }
}