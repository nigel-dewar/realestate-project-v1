using System;
using System.Threading.Tasks;
using Manage.Listings.Processor.Services.Interfaces;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Events;

namespace Manage.Listings.Processor.Messages
{
    public class ListingPublishEventConsumer: IConsumer<IListingPublishEvent>
    {

        private IListingProcessor _listingProcessor;

        public ListingPublishEventConsumer(IListingProcessor listingProcessor)
        {
            _listingProcessor = listingProcessor;
        }

        public async Task Consume(ConsumeContext<IListingPublishEvent> context)
        {
            var message = context.Message;
            
            var correlationId = message.CorrelationId;
            var listingId = message.ManageListingId;
            var processorAction = message.ProcessorAction;

            await _listingProcessor.ReceiveElasticSearchEvent(correlationId, listingId, processorAction);

        }
    }
}