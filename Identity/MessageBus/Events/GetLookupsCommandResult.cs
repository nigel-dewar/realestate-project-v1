using System.Collections.Generic;

namespace MessageBus.Events
{
    public class GetLookupsCommandResult: IGetLookupsCommandResult
    {
        public List<string> Features { get; set; }
        
        public List<string> PropertyTypes { get; set; }
    }
}