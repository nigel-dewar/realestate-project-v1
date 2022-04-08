using System.Collections.Generic;

namespace Search.Common.Models
{
    public class FindListingMapped
    {
        public string Mode { get; set; }

        public int Page { get; set; }

        public string Suburbs { get; set; }

        public string PropertyTypes { get; set; }

        public string Features { get; set; }

        public List<int> Price { get; set; }
        
        public List<int> BedRooms { get; set; }

        public List<int> BathRooms { get; set; }

        public List<int> Parking { get; set; }
    }
}