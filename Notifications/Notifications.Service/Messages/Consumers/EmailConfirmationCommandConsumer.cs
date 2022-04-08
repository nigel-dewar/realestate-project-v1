using System;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Commands;
using Notifications.Email;
using Serilog;

namespace Notifications.Service.Messages.Consumers
{
    public class EmailConfirmationCommandConsumer: IConsumer<IEmailConfirmationCommand>
    {
        private IEmailService _mailService;
        private ILogger _logger;

        public EmailConfirmationCommandConsumer(IEmailService mailService, ILogger logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IEmailConfirmationCommand> context)
        {
            try
            {
                var result = context.Message;

                await _mailService.SendListingConfirmationEmail(result);

                // TODO - SEND Out Success Event


            }
            catch (Exception e)
            {
                _logger.Error(e.Message,$"Error attempting to Send Confirmation Email: {context.Message.ToEmail} CorrelationId: {context.Message.CorrelationId}");
                throw;
            }
            
        }
    }
}