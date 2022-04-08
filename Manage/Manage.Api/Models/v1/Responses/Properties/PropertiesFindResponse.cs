using System.Collections.Generic;

namespace Manage.API.Models.v1.Responses.Properties
{
    public class PropertiesFindResponse
    {
        public int Total { get; set; }

        public int AvailablePages { get; set; }

        public int CurrentPageNumber { get; set; }


        public List<PropertyResponse> Properties { get; set; }
    }
}
