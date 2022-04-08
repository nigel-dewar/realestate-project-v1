using System.Collections.Generic;

namespace Search.Common.Models
{
    public class ListingModel
    {
        public string Id { get; set; }

        public string ManageListingId { get; set; }
        
        public string Slug { get; set; }
        
        public string Title { get; set; }

        public string Address { get; set; }

        public string ListingRef { get; set; }

        public string PropertyId { get; set; }

        public string DateCreated { get; set; } 
        
        public string DateListingStarts { get; set; }
        
        public string DateListingExpires { get; set; } // date the listing will expire
        
        public string ContactEmail { get; set; } // used to send emails to lister

        public string ContactNumber { get; set; } // can be landline or mobile

        // Only 1 type of listing can be active at once. For example, only 1 rent type can be active at once
        // Only 1 type of Buy listing can be active at once
        
        // Asking price
        public int Price { get; set; } 

        public string ListingType { get; set; } // rent, buy, auction etc

        public bool IsPremium { get; set; }  // default is standard

        public string CreatedById { get; set; }

        

        public string SuburbSlug { get; set; }

        public string PropertyType { get; set; }
        
        public int PropertyTypeId { get; set; }
        
        public string Description { get; set; }

        public List<string> Features { get; set; }

        public int Bedrooms { get; set; }
        
        public int Bathrooms { get; set; }
        
        public int ParkingSpaces { get; set; }
        
        public int LandSize { get; set; }

        // Image
        
        public string MainImage { get; set; }
        
        public List<string> Images { get; set; }


        // Address Info
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

    }
}