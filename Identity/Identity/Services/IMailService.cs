using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Identity.API.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }

    public class SendGridMailService : IMailService
    {

        private readonly IConfiguration _configuration;
        
        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string toSubject, string content)
        {
            // toEmail = "nigel.dewar@protonmail.com";
            try
            {
                var apiKey = _configuration["EmailSettings:SendGridApiKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("accounts@realestateify.com", "Realestateify Account Confirmation");
                var subject = toSubject;
                var to = new EmailAddress(toEmail);
                // var plainTextContent = "and easy to do anywhere, even with C#";
                // var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "Please confirm account", content);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}