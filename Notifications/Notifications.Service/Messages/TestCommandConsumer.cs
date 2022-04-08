using System;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Commands;

namespace Notifications.Service.Messages
{
    public class TestCommandConsumer: IConsumer<ITestPublishCommand>
    {
        public async Task Consume(ConsumeContext<ITestPublishCommand> context)
        {
            var message = context.Message;
            Guid orderId = message.TestPublishId;
            
            // await _mailService.SendListingConfirmationEmail(dto.EmailAddress,
            //     $"Estateico: Congrats! Your new Ad Ref: {dto.AdRef} for {dto.PropertyName} , for has been listed successfully.",
            //     $"You can visit your ad here at {dto.Url}");

        }
    }
}