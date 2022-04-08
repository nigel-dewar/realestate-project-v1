using System;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Commands;

namespace Manage.Listings.Processor.Messages
{
    public class TestCommandConsumer: IConsumer<ITestPublishCommand>
    {
        public async Task Consume(ConsumeContext<ITestPublishCommand> context)
        {
            var message = context.Message;
            Guid orderId = message.TestPublishId;
            
            
        }
    }
}