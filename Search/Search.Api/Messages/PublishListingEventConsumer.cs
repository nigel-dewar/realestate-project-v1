using System;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Events;
using MessageBus.Models;
using Microsoft.Extensions.Logging;
using Search.Elastic.Service.Services.Interfaces;

namespace Search.Elastic.Service.Messages
{
    public class ListingPublishCommandConsumer: IConsumer<IListingPublishCommand>
    {
        // private ILogger _logger;
        private readonly IListingsService _listingsService;

        public ListingPublishCommandConsumer(IListingsService listingsService)
        {
            _listingsService = listingsService;
        }

        public async Task Consume(ConsumeContext<IListingPublishCommand> context)
        {
            try
            {
                var result = context.Message;

                await _listingsService.SyncListing(result);

                // var id = result.ListingId;

                await context.Publish<IListingPublishEvent>(new ListingPublishEvent
                {
                    CorrelationId = result.RequestId.ToString(),
                    ProcessorAction = result.ProcessorAction,
                    ManageListingId = result.ListingId
                });
            
                Console.WriteLine("Message received to Search Elastic Service");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            

        }
    }
}