using System;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Events;
using Notifications.Email;
using Notifications.SMS;
using Serilog;

namespace Notifications.Service.Messages.Consumers
{
    public class SmsVerifcationCommandConsumer: IConsumer<ISmsVerificationCommand>
    {
        private ISmsService _smsService;
        private ILogger _logger;

        public SmsVerifcationCommandConsumer(ILogger logger, ISmsService smsService)
        {
            _logger = logger;
            _smsService = smsService;
        }

        public async Task Consume(ConsumeContext<ISmsVerificationCommand> context)
        {
            try
            {
                var result = context.Message;
                
                

                var response = _smsService.SendVerificationCode(result.Number, result.Code);


                await context.RespondAsync<ISmsVerificationCommandResult>(new SmsVerificationCommandResult()
                {
                    SmsSent = response
                });


            }
            catch (Exception e)
            {
                _logger.Error(e.Message,$"Error attempting to Send Verifcation SMS: {context.Message.Number}");
            }
            
        }
    }
}