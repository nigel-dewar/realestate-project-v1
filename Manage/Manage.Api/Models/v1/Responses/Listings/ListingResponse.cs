using System;
using Manage.API.Models.v1.Responses.Agencies;
using Manage.API.Models.v1.Responses.Agents;

namespace Manage.API.Models.v1.Responses.Listings
{
    public class ListingResponse
    {
        public string Id { get; set; }

        public string ListingRef { get; set; }

        public string PropertyId { get; set; }
        

        public DateTimeOffset DateCreated { get; set; }

        public DateTimeOffset DateListingExpires { get; set; } // date the listing will expire

        // Only 1 type of listing can be active at once. For example, only 1 rent type can be active at once
        // Only 1 type of Buy listing can be active at once
        public bool Active { get; set; } 

        // Asking price
        public decimal Price { get; set; }

        public string ListingType { get; set; } // rent, buy, auction etc

        public bool IsPremium { get; set; } // default is standard

        // username who created to listing
        public string UserName { get; set; }

        public string UserId { get; set; }

        public bool IsListedByAgent { get; set; } 

        public bool IsPrivateSeller { get; set; }  // default is not private seller

        public string AgencyId { get; set; } // reference to Agency
        
        public string AgentId { get; set; }
        
        public AgencyResponse Agency { get; internal set; }
        public AgentResponse Agent { get; internal set; }
    }
}