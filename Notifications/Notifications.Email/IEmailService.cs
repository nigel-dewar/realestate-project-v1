using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBus.Commands;
using Notifications.Email.Models;

namespace Notifications.Email
{
    public interface IEmailService
    {
        Task SendListingConfirmationEmail(IEmailConfirmationCommand request);
        
        Task SendWelcomeEmail(IEmailWelcomeCommand request);
        
        Task SendGenericEmail(IEmailGenericCommand request);

        void MailTest();
        
        void Send(EmailMessage emailMessage);
        
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}