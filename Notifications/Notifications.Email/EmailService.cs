using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MessageBus.Commands;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Notifications.Email.Models;
using Notifications.Email.Models.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using EmailAddress = Notifications.Email.Models.EmailAddress;

// using EmailAddress = SendGrid.Helpers.Mail.EmailAddress;

namespace Notifications.Email
{
    public class EmailService : IEmailService
    {
        
        private readonly IConfiguration _configuration;
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IConfiguration configuration, IEmailConfiguration emailConfiguration)
        {
            _configuration = configuration;
            _emailConfiguration = emailConfiguration;

        }
        
        public async Task SendListingConfirmationEmail(IEmailConfirmationCommand request)
        {
            try
            {
                // var apiKey = _configuration["EmailSettings:SendGridApiKey"];
                // var client = new SendGridClient(apiKey);
                //
                // var from = new EmailAddress("accounts@realestateify.com", "Realestateify");
                // var subject = "Please Confirm your email";
                // var to = new EmailAddress(request.ToEmail);
                // // var plainTextContent = "and easy to do anywhere, even with C#";
                // var htmlContent = $"<h1>Welcome to Realestateify.</h1> <p>confirm your new account here </p><br><br> <a href='{request.ConfirmEmailUrl}'>Click here</a>";
                // var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
                // var response = await client.SendEmailAsync(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SendWelcomeEmail(IEmailWelcomeCommand request)
        {
            // try
            // {
            //     var apiKey = _configuration["EmailSettings:SendGridApiKey"];
            //     var client = new SendGridClient(apiKey);
            //     
            //     var from = new EmailAddress("accounts@realestateify.com", "Realestateify");
            //     var subject = "Welcome to ";
            //     var to = new EmailAddress(request.ToEmail);
            //     // var plainTextContent = "and easy to do anywhere, even with C#";
            //     var htmlContent = $"<h1>Welcome to realestateify.com</h1> <p>Welcome. </p><br><br> Login at <a href='{_configuration["AppSettings:FrontEndUrl"]}'>Click here</a>";
            //     var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            //     var response = await client.SendEmailAsync(msg);
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     throw;
            // }

            
        }
        
        public async Task SendGenericEmail(IEmailGenericCommand request)
        {
            try
            {
                var emailMessage = new EmailMessage
                {
                    ToAddresses = new List<EmailAddress>()
                    {
                        new EmailAddress()
                        {
                            Address = request.ToEmail,
                            Name = request.ToName
                        }
                    },
                    Content = request.Message,
                    Subject = request.Subject,
                    FromAddresses = new List<EmailAddress>()
                    {
                        new EmailAddress()
                        {
                            Address = _emailConfiguration.SmtpUsername,
                            Name = _emailConfiguration.SmtpAccountName
                        }
                    }

                };
                
                var message = new MimeMessage();
                message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.Subject = emailMessage.Subject;
                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = emailMessage.Content
                };

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    // emailClient.CheckCertificateRevocation = false;
                    emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, SecureSocketOptions.StartTlsWhenAvailable);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                    emailClient.Send(message);

                    emailClient.Disconnect(true);
                }
                // var apiKey = _configuration["EmailSettings:SendGridApiKey"];
                // var client = new SendGridClient(apiKey);
                //
                // var from = new EmailAddress("accounts@realestateify.com", "Realestateify");
                // var subject = request.Subject;
                // var to = new EmailAddress(request.ToEmail);
                // // var plainTextContent = "and easy to do anywhere, even with C#";
                // var htmlContent = request.Message;
                // var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
                // var response = await client.SendEmailAsync(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public void MailTest()
        {
            try
            {
                // _EmailService.Send("nigel@realestateify.com", "Test MailKit Extensions", "<b>Hi There</b>", true);
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress("Realestateify","nigel@realestateify.com"));
                mailMessage.To.Add(new MailboxAddress("Nigel", "nigeldewar@live.com"));
                mailMessage.Subject = "Account email";
                mailMessage.Body = new TextPart("This is some info")
                {
                    Text = "This is some info"
                };
                
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect("mail.realestateify.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
                    smtpClient.Authenticate("nigel@realestateify.com", "Austina40!");
                    smtpClient.Send(mailMessage);
                    smtpClient.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Send(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, SecureSocketOptions.StartTlsWhenAvailable);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            // Implement POP recieve from this article
            // https://dotnetcoretutorials.com/2017/11/02/using-mailkit-send-receive-email-asp-net-core/
            throw new NotImplementedException();
        }
    }
}