using System.Collections.Generic;

namespace Manage.API.Models.v1.Requests.Listings
{
    public class SaveListingPropertyRequest
    {
        public string Name { get; set; }
        
        public int PropertyTypeId { get; set; }
        
        public string Description { get; set; }

        public int Bedrooms { get; set; }
        
        public int Bathrooms { get; set; }
        
        public int ParkingSpaces { get; set; }
        
        public decimal LandSize { get; set; }
        
        // Property Address Info
        public string StreetNumber { get; set; }
        public string StreetName { get; set; } // Was route
        public string PostalCode { get; set; } // Was PostCode
        public string SuburbOrCity { get; set; } // Was locality
        public string Council { get; set; } // Was state otherwise known as administrative_area_level_2
        public string RegionOrState { get; set; } // was state otherwise known as administrative_area_level_1
        public string Country { get; set; }
        
        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public string GoogleAddress { get; set; }
        
        public List<PropertyFeaturesRequest> Features { get; set; }
    }
}