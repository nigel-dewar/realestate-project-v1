using System;
using System.Threading.Tasks;
using Hangfire;
using MassTransit;
using MessageBus.Constants;
using MessageBus.Events;
using Microsoft.AspNetCore.Mvc;

namespace Manage.Listings.Processor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesterController : ControllerBase
    {

        private readonly IBusControl _busControl;

        public TesterController(IBusControl busControl)
        {
            _busControl = busControl;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var correlationId = Guid.NewGuid().ToString();
            
            await SendElasticCommand(correlationId);
            
            // await SendMyqlCommand(correlationId);
            //             
            // await SendRethinkCommand(correlationId);

            return Ok("Sent");
        }
        
        private async Task SendElasticCommand(string correlationId)
        {
            var sentToUri =
                new Uri($"{EventBusConstants.RabbitMqUri}" + $"{EventBusConstants.PublishListingToElasticCommandQueue}");

            var endPoint = await _busControl.GetSendEndpoint(sentToUri);
            await endPoint.Send<IListingPublishEvent>(
                new
                {
                    CorrelationId = correlationId,
                    Id = "4654564-54654654564564-656545564"
                }
            );
        }
        
        private async Task SendMyqlCommand(string correlationId)
        {
            var sentToUri =
                new Uri($"{EventBusConstants.RabbitMqUri}" + $"{EventBusConstants.PublishListingToMysqlCommandQueue}");

            var endPoint = await _busControl.GetSendEndpoint(sentToUri);
            await endPoint.Send<IListingPublishEvent>(
                new
                {
                    CorrelationId = correlationId,
                    Id = "465465-4654645"
                }
            );

        }
        
        private async Task SendRethinkCommand(string correlationId)
        {
            var sentToUri =
                new Uri($"{EventBusConstants.RabbitMqUri}" + $"{EventBusConstants.PublishListingToRethinkCommandQueue}");

            var endPoint = await _busControl.GetSendEndpoint(sentToUri);
            await endPoint.Send<IListingPublishEvent>(
                new
                {
                    CorrelationId = correlationId,
                    Id = "4654564-54654654564564-656545564"
                }
            );
        }
        
    }
}