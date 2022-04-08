using System;
using System.Collections.Generic;
using Manage.API.Models.v1.DTO;
using Manage.API.Models.v1.Responses.Agencies;
using Manage.API.Models.v1.Responses.Agents;
using Manage.Repository.Entities;

namespace Manage.API.Models.v1.Responses.Listings
{
    public class ListingWithPropertyResponse
    {
        public string Id { get; set; }
        
        public string ListingRef { get; set; }

        public bool Confirmed { get; set; }

        public string PropertyId { get; set; }

        public DateTimeOffset DateCreated { get; set; } 
        
        public DateTimeOffset DateListingStarts { get; set; }
        
        public DateTimeOffset DateListingExpires { get; set; } // date the listing will expire
        
        public string ContactEmail { get; set; } // used to send emails to lister

        public string ContactNumber { get; set; } // can be landline or mobile

        public bool ShowNumber { get; set; }

        // Only 1 type of listing can be active at once. For example, only 1 rent type can be active at once
        // Only 1 type of Buy listing can be active at once
        public bool Active { get; set; }
        
        public StatusCode StatusCode { get; set; }

        public bool Complete { get; set; }

        public bool Cancelled { get; set; }

        public string Status { get; set; }

        public bool IsPublishedLive { get; set; }

        // Asking price
        public decimal Price { get; set; } 

        public string ListingType { get; set; } // rent, buy, auction etc

        public bool IsPremium { get; set; }  // default is standard

        // username who created to listing
        public string UserName { get; set; }

        public string UserId { get; set; }

        public bool IsListedByAgent { get; set; } 

        public bool IsPrivateSeller { get; set; }  // default is not private seller

        public string AgencyId { get; set; } // reference to Agency
        
        public string AgentId { get; set; }

        public string CreatedById { get; set; }

        public string Name { get; set; }
        
        public bool CanEditAddress { get; set; }
        public int AddressChanged { get; set; }
        
        public string Slug { get; set; }

        public string SuburbSlug { get; set; }

        public int PropertyTypeId { get; set; }
        
        public string Description { get; set; }

        public int Bedrooms { get; set; }
        
        public int Bathrooms { get; set; }
        
        public int ParkingSpaces { get; set; }
        
        public decimal LandSize { get; set; }

        public List<PropertyFeatureResponse> Features { get; set; }
        
        // Image
        
        public string MainImage { get; set; }
        
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

        public CompletenessCheckerDto Checker { get; set; } = new CompletenessCheckerDto();
        
        
        public AgencyResponse Agency { get; internal set; }
        public AgentResponse Agent { get; internal set; }
    }
}