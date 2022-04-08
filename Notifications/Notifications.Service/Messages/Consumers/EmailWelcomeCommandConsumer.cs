using System;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Commands;
using Notifications.Email;
using Serilog;

namespace Notifications.Service.Messages.Consumers
{
    public class EmailWelcomeCommandConsumer: IConsumer<IEmailWelcomeCommand>
    {
        private IEmailService _mailService;
        private ILogger _logger;

        public EmailWelcomeCommandConsumer(IEmailService mailService, ILogger logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IEmailWelcomeCommand> context)
        {
            try
            {
                var result = context.Message;

                await _mailService.SendWelcomeEmail(result);

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