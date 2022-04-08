using System.Collections.Generic;

namespace Search.Common.Models.Requests
{
    public class FindListingRequest
    {
        public string Mode { get; set; }

        public int Page { get; set; }

        public string Suburbs { get; set; }

        public string PropertyTypes { get; set; }

        public string Features { get; set; }

        public string Price { get; set; }
        
        public string BedRooms { get; set; }

        public string BathRooms { get; set; }

        public string Parking { get; set; }
    }
}
