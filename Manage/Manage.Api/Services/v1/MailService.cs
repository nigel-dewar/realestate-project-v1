using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Manage.API.Interfaces;
using Manage.API.Models.v1.Responses.PostCodes;
using Manage.Repository.Data;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Constants;
using MessageBus.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Manage.API.Services.v1
{
    public class MailService: IMailService
    {
        private readonly ManageDataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger _logger;
        private readonly IBusControl _busControl;
        private readonly IConfiguration _configuration;

        public MailService(ManageDataContext context, IUserAccessor userAccessor, ILogger logger, IBusControl busControl, IConfiguration configuration)
        {
            _context = context;
            _userAccessor = userAccessor;
            _logger = logger;
            _busControl = busControl;
            _configuration = configuration;
        }

        public async Task EmailMessage(EmailGenericCommand message)
        {
            // Send Welcome email to user
            try
            {
                var rabbitUri = $"rabbitmq://{_configuration["EventBus:HostName"]}/";
                var sentToUri = new Uri(rabbitUri + $"{EventBusConstants.EmailGenericCommand}");

                var endPoint = await _busControl.GetSendEndpoint(sentToUri);
                await endPoint.Send<IEmailGenericCommand>(
                    message
                );
            }
            catch (Exception e)
            {
                _logger.Error($"Could not setup MessageBus to send Generic Email");
                throw;
            }
        }
    }
}