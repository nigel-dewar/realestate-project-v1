// using System;
// using System.Threading.Tasks;
// using MessageBus.Commands;
// using Microsoft.Extensions.Configuration;
// using SendGrid;
// using SendGrid.Helpers.Mail;
//
// namespace Notifications.Email
// {
//     public class MailService : IMailService
//     {
//         
//         private readonly IConfiguration _configuration;
//         
//         public MailService(IConfiguration configuration)
//         {
//             _configuration = configuration;
//         }
//         
//         public async Task SendListingConfirmationEmail(IEmailConfirmationCommand request)
//         {
//             try
//             {
//                 var apiKey = _configuration["EmailSettings:SendGridApiKey"];
//                 var client = new SendGridClient(apiKey);
//                 
//                 var from = new EmailAddress("accounts@realestateify.com", "Realestateify");
//                 var subject = "Please Confirm your email";
//                 var to = new EmailAddress(request.ToEmail);
//                 // var plainTextContent = "and easy to do anywhere, even with C#";
//                 var htmlContent = $"<h1>Welcome to Realestateify.</h1> <p>confirm your new account here </p><br><br> <a href='{request.ConfirmEmailUrl}'>Click here</a>";
//                 var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
//                 var response = await client.SendEmailAsync(msg);
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine(e);
//                 throw;
//             }
//         }
//
//         public async Task SendWelcomeEmail(IEmailWelcomeCommand request)
//         {
//             try
//             {
//                 var apiKey = _configuration["EmailSettings:SendGridApiKey"];
//                 var client = new SendGridClient(apiKey);
//                 
//                 var from = new EmailAddress("accounts@realestateify.com", "Realestateify");
//                 var subject = "Welcome to ";
//                 var to = new EmailAddress(request.ToEmail);
//                 // var plainTextContent = "and easy to do anywhere, even with C#";
//                 var htmlContent = $"<h1>Welcome to realestateify.com</h1> <p>Welcome. </p><br><br> Login at <a href='{_configuration["AppSettings:FrontEndUrl"]}'>Click here</a>";
//                 var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
//                 var response = await client.SendEmailAsync(msg);
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine(e);
//                 throw;
//             }
//         }
//         
//         public async Task SendGenericEmail(IEmailGenericCommand request)
//         {
//             try
//             {
//                 var apiKey = _configuration["EmailSettings:SendGridApiKey"];
//                 var client = new SendGridClient(apiKey);
//                 
//                 var from = new EmailAddress("accounts@realestateify.com", "Realestateify");
//                 var subject = request.Subject;
//                 var to = new EmailAddress(request.ToEmail);
//                 // var plainTextContent = "and easy to do anywhere, even with C#";
//                 var htmlContent = request.Message;
//                 var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
//                 var response = await client.SendEmailAsync(msg);
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine(e);
//                 throw;
//             }
//         }
//     }
// }