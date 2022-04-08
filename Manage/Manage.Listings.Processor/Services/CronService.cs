using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Storage;
using Manage.Listings.Processor.Services.Interfaces;

namespace Manage.Listings.Processor.Services
{
    public class CronService: ICronService
    {
        
        private static IListingProcessor _converter;

        public CronService(IListingProcessor converter)
        {
            _converter = converter;
        }

        public void SetupConversion()
        {
            var jobName = "process-listings";
            Console.WriteLine("Conversion Service has started");
            try
            {
                var connection = JobStorage.Current.GetConnection();
                
                    string lastRunResult = string.Empty;
                    var recurringJobs = connection.GetRecurringJobs();
                    var job = recurringJobs.FirstOrDefault(p =>
                        p.Id.Equals(jobName, StringComparison.InvariantCultureIgnoreCase));
                    if (job != null)
                    {

                        try
                        {
                            var jobState = connection.GetStateData(job.LastJobId);
                            lastRunResult = jobState.Name; // For Example: "Succeeded", "Processing", "Deleted"
                            // RecurringJob.AddOrUpdate(jobName,() => StartConversion(),
                            //     Cron.Minutely);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            // throw;
                            //job has not been run by the scheduler yet, swallow error
                        }
                    }
                    else
                    {
                        // there is no job running
                        Console.WriteLine("Time to start the job");
                        RecurringJob.AddOrUpdate(jobName,() => StartConversion(),
                            Cron.Minutely);
                        Console.WriteLine("Job is now started - YEAH!");
                    }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task StartConversion()
        {
            try
            {
                var correlationId = await _converter.ProcessNewListings();
                Console.WriteLine($"Conversion Job Processed - CorrelationId: {correlationId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}