using System.Collections.Generic;

namespace MessageBus.Events
{
    public interface IGetLookupsCommandResult
    {
        List<string> Features { get; set; }
        
        List<string> PropertyTypes { get; set; }
        
    }
}