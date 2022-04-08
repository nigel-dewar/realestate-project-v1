using System;

namespace Manage.API.Models.v1.Responses.Listings
{
    public class PropertyOnlyResponse
    {
        public string Id { get; set; }
        
        public string CreatedById { get; set; }

        public DateTime Created { get; set; }
        
        public string Name { get; set; }
        
        public string Slug { get; set; }

        public string SuburbSlug { get; set; }

        public int PropertyTypeId { get; set; }
        
        public string Description { get; set; }

        public int Bedrooms { get; set; }
        
        public int Bathrooms { get; set; }
        
        public int ParkingSpaces { get; set; }
        
        public decimal LandSize { get; set; }
        
        // Address Info
        public string StreetNumber { get; set; }
        public string StreetName { get; set; } // Was route
        public string PostalCode { get; set; } // Was PostCode
        public string SuburbOrCity { get; set; } // Was locality
        public string Council { get; set; } // Was state otherwise known as administrative_area_level_2
        public string RegionOrState { get; set; } // was state otherwise known as administrative_area_level_1
        public string Country { get; set; }
        
        // Fields for Search functions. Filled out during Listing process

    }
}