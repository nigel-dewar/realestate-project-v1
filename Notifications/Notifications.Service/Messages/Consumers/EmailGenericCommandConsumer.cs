using System;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Commands;
using Notifications.Email;
using Serilog;

namespace Notifications.Service.Messages.Consumers
{
    public class EmailGenericCommandConsumer: IConsumer<IEmailGenericCommand>
    {
        private IEmailService _mailService;
        private ILogger _logger;

        public EmailGenericCommandConsumer(IEmailService mailService, ILogger logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IEmailGenericCommand> context)
        {
            try
            {
                var result = context.Message;

                await _mailService.SendGenericEmail(result);

                // OPTIONAL - SEND Out Success Event

            }
            catch (Exception e)
            {
                _logger.Error(e.Message,$"Error attempting to Send Generic Email: {context.Message.ToEmail} CorrelationId: {context.Message.CorrelationId}");
                throw;
            }
            
        }
    }
}