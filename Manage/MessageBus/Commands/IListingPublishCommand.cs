using System;
using System.Collections.Generic;

namespace MessageBus.Commands
{
    public interface IListingPublishCommand
    {
        Guid RequestId { get; set; }
        
        string ProcessorAction { get; set; }
        
        string ListingId { get; set; }
        
        string Slug { get; set; }
        
        string Title { get; set; }

        string Address { get; set; }

        string ListingRef { get; set; }

        string PropertyId { get; set; }

        string DateCreated { get; set; } 
        
        string DateListingStarts { get; set; }
        
        string DateListingExpires { get; set; } // date the listing will expire
        
        string ContactEmail { get; set; } // used to send emails to lister

        string ContactNumber { get; set; } // can be landline or mobile

        // Only 1 type of listing can be active at once. For example, only 1 rent type can be active at once
        // Only 1 type of Buy listing can be active at once
        
        // Asking price
        int Price { get; set; } 

        string ListingType { get; set; } // rent, buy, auction etc

        bool IsPremium { get; set; }  // default is standard

        string CreatedById { get; set; }

        

        string SuburbSlug { get; set; }

        string PropertyType { get; set; }
        
        int PropertyTypeId { get; set; }
        
        string Description { get; set; }

        List<string> Features { get; set; }

        int Bedrooms { get; set; }
        
        int Bathrooms { get; set; }
        
        int ParkingSpaces { get; set; }
        
        int LandSize { get; set; }

        // Image
        
        string MainImage { get; set; }
        
        List<string> Images { get; set; }


        // Address Info
        string StreetNumber { get; set; }
        
        string StreetName { get; set; } // Was route
        
        string PostalCode { get; set; } // Was PostCode
        
        string SuburbOrCity { get; set; } // Was locality
        
        string Council { get; set; } // Was state otherwise known as administrative_area_level_2
        
        string RegionOrState { get; set; } // was state otherwise known as administrative_area_level_1
        
        string Country { get; set; }
        
        string Longitude { get; set; }

        string Latitude { get; set; }

        string GoogleAddress { get; set; }
    }
}