using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Notifications.Email;
using Notifications.Email.Models;
using Notifications.Email.Models.Interfaces;
using Notifications.Service.Models;

namespace Notifications.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmailController : ControllerBase
    {
        
        private IEmailService _mailService;
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailController(IEmailService mailService, IEmailConfiguration emailConfiguration)
        {
            _mailService = mailService;
            _emailConfiguration = emailConfiguration;
        }

        // GET api/values
        [HttpPost]
        public ActionResult<bool> Test(EmailRequest request)
        {


            try
            {
                _mailService.Send(new EmailMessage()
                {
                    ToAddresses = new List<EmailAddress>()
                    {
                        new EmailAddress()
                        {
                            Address = "nigeldewar@live.com",
                            Name = "Nigel Dewar"
                        }
                    },
                    Content = "Hi This is a message from Nigel",
                    Subject = "Wow this actually worked",
                    FromAddresses = new List<EmailAddress>()
                    {
                        new EmailAddress()
                        {
                            Address = _emailConfiguration.SmtpUsername,
                            Name = _emailConfiguration.SmtpAccountName
                        }
                    }
                    
                });
                // var mailMessage = new MimeMessage();
                // mailMessage.From.Add(new MailboxAddress("Realestateify","nigel@realestateify.com"));
                // mailMessage.To.Add(new MailboxAddress("Nigel", "nigeldewar@live.com"));
                // mailMessage.Subject = "Account email";
                // mailMessage.Body = new TextPart("This is some info")
                // {
                //     Text = "This is some info"
                // };
                //
                // using (var smtpClient = new SmtpClient())
                // {
                //     smtpClient.Connect("mail.realestateify.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
                //     smtpClient.Authenticate("dev-noreply@realestateify.com", "SomePassword");
                //     smtpClient.Send(mailMessage);
                //     smtpClient.Disconnect(true);
                // }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
            
            
            return true;
        }
        
        [HttpGet]
        public ActionResult<string> TestString()
        {

            try
            {
                
                return "it works";

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        
        
        
        
    }
}