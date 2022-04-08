using System;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Notifications.SMS
{
    public class SmsService: ISmsService
    {
    
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendVerificationCode(string number, string code)
        {
            string accountSid = _configuration["SmsSettings:AccountSid"];
            string authToken = _configuration["SmsSettings:AuthToken"];

            // Initialize the Twilio client
            TwilioClient.Init(accountSid, authToken);

            // make an associative array of people we know, indexed by phone number
            // var people = new Dictionary<string, string>() {
            //     {"+61414218764", "Nigel Dewar"},
            //     {"+61450258658", "Neeka Dewar"}
            // };

            // Iterate over all our friends
            // foreach (var person in people)
            // {
            //     // Send a new outgoing SMS by POSTing to the Messages resource
            //     MessageResource.Create(
            //         from: new PhoneNumber("+12058556995"), // From number, must be an SMS-enabled Twilio number
            //         to: new PhoneNumber(person.Key), // To number, if using Sandbox see note above
            //         // Message content
            //         body: $"Hey {person.Value} This is a message that Nigington Loves you very much!!!");
            //
            //     Console.WriteLine($"Sent message to {person.Value}");
            // }

            try
            {
                MessageResource.Create(
                    from: new PhoneNumber(_configuration["SmsSettings:PhoneNumber"]), // From number, must be an SMS-enabled Twilio number
                    to: new PhoneNumber(number), // To number, if using Sandbox see note above
                    // Message content
                    body: $"Your Realestateify Verification Code is {code}");
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }
    }
}