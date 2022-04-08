
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Manage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IBusControl _busControl;

        public ValuesController(IBusControl busControl)
        {
            _busControl = busControl;
        }

        // GET api/values
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var sentToUri = new Uri($"{EventBusConstants.RabbitMqUri}" + $"{EventBusConstants.TestPublishQueue}");

            var endPoint = await _busControl.GetSendEndpoint(sentToUri);
            await endPoint.Send<ITestPublishCommand>(
                new
                {
                    TestPublishId = Guid.NewGuid(),
                    TestMessage = "Yay I was sent"
                }
            );
            
            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}