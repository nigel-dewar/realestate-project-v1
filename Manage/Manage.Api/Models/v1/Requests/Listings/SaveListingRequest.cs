using System.Collections.Generic;

// RULES
// Only 1 type of listing can be active at once. For example, only 1 rent type can be active at once
// Only 1 type of Buy listing can be active at once

namespace Manage.API.Models.v1.Requests.Listings
{
    public class SaveListingRequest
    {
        // Listings section
        public string Id { get; set; }

        public string UserRequestAction { get; set; }
        public decimal Price { get; set; }
        public string ListingType { get; set; } // rent, buy, auction etc
        public bool IsPremium { get; set; } // defaults to false
        
        public long DateListingStarts { get; set; }
        
        public string ContactEmail { get; set; } 

        public string ContactNumber { get; set; }

        public bool ShowNumber { get; set; }
        
        // Property section

        public string PropertyId { get; set; }

        public int PropertyTypeId { get; set; }
        
        public string Description { get; set; }

        public int Bedrooms { get; set; }
        
        public int Bathrooms { get; set; }
        
        public int ParkingSpaces { get; set; }
        
        public int LandSize { get; set; }
        
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