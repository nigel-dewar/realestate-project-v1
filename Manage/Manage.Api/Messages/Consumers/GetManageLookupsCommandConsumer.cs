using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage.API.Interfaces;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Events;
using Serilog;

namespace Manage.API.Messages.Consumers
{
    public class GetManageListingsCommandConsumer: IConsumer<IGetLookupsCommand>
    {
        private ILogger _logger;
        private IListingService _listingService;

        public GetManageListingsCommandConsumer(ILogger logger, IListingService listingService)
        {
            _logger = logger;
            _listingService = listingService;
        }

        public async Task Consume(ConsumeContext<IGetLookupsCommand> context)
        {
            try
            {

                var response = await _listingService.GetLookups();

                var features = new List<string>();
                var propertyTypes = new List<string>();

                foreach (var feature in response.Features)
                {
                    features.Add(feature.Name);
                }
                
                foreach (var propertyType in response.PropertyTypes)
                {
                    propertyTypes.Add(propertyType.Type);
                }

                await context.RespondAsync<IGetLookupsCommandResult>(new GetLookupsCommandResult()
                {
                    Features = features,
                    PropertyTypes = propertyTypes
                });
                
            }
            catch (Exception e)
            {
                _logger.Error(e.Message,$"Error Processing GetLookups Consumer at Manage API");
            }
            
        }
    }
}