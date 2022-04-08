using System;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Manage.Listings.Processor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("hello from hangfire");
        }
        
        [HttpPost("welcome")]
        public IActionResult Welcome()
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to our app"));

            return Ok($"JobID: {jobId} . - Welcome email sent to user");
        }
        
        [HttpPost("discount")]
        public IActionResult Discount()
        {
            var jobId = BackgroundJob.Schedule(() => SendWelcomeEmail("Applied discount"), TimeSpan.FromSeconds(30));

            return Ok($"JobID: {jobId} . - Applying discount very soon");
        }
        
        [HttpPost("report")]
        public IActionResult Report()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("report updated"), Cron.Minutely);

            return Ok("report done yeah");
        }
        
        [HttpPost("confirm")]
        public IActionResult Confirm()
        {
            var parentJobId = BackgroundJob.Schedule(() => SendWelcomeEmail("You asked to be unsubscribed"), TimeSpan.FromSeconds(30));

            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You are unsubscribed"));

            return Ok("Confirmation job created");
        }

        public void SendWelcomeEmail(string text)
        {
            Console.WriteLine(text);
        }
    }
}