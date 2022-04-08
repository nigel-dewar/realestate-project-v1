using System.Collections.Generic;

namespace Search.Common.Models.Responses
{
    public class ManageLookupsResponse
    {
        public IEnumerable<ManagePropertyType> PropertyTypes { get; set; }
        public IEnumerable<ManageFeature> Features { get; set; }
    }
}