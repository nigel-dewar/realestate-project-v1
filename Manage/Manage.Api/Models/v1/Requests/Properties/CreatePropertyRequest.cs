namespace Manage.API.Models.v1.Requests.Properties
{
    public class CreatePropertyRequest
    {
        public int PropertyTypeId { get; set; }

        public int Bedrooms { get; set; }
        
        public int Bathrooms { get; set; }
        
        public int ParkingSpaces { get; set; }
        
        public int LandSize { get; set; }
        
        public string StreetNumber { get; set; }
        
        public string StreetName { get; set; }
        
        public string PostalCode { get; set; }
        
        public string SuburbOrCity { get; set; } // Was locality
        
        public string Council { get; set; } // was state otherwise known as administrative_area_level_2
        
        public string RegionOrState { get; set; } // was state otherwise known as administrative_area_level_1
        
        public string Country { get; set; }
        
        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public string GoogleAddress { get; set; }

        public string ListingType { get; set; }
    }
}