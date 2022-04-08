using System;
using System.Collections.Generic;
using Manage.API.Models.v1.Responses.Activities;
using Manage.API.Models.v1.Responses.Images;
using Manage.API.Models.v1.Responses.Listings;
using Newtonsoft.Json;

namespace Manage.API.Models.v1.Responses.Properties
{
    public class PropertyResponse
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
        
        public bool IsActive { get; set; } = false; 
        public bool IsForSale { get; set; } = false;
        public bool IsForRent { get; set; } = false;
        public bool IsForAuction { get; set; } = false;
        public decimal BuyPrice { get; set; }
        public decimal RentPrice { get; set; }
        public DateTimeOffset AvailableDate { get; set; }
        
        
        // [JsonProperty("propertyType")]
        // public virtual PropertyTypeDto PropertyType { get; set; }
        
        // public virtual List<PropertyFeatureDto> PropertyFeatures { get; set; } = new List<PropertyFeatureDto>();
        //
        
        [JsonProperty("listings")]
        public ICollection<ListingResponse> Listings { get; set; }
        
        [JsonProperty("images")]
        public List<CreateImageResponse> Images { get; set; }
        
        // public virtual List<PropertyPermissionDto> PropertyPermissions { get; set; } = new List<PropertyPermissionDto>();
        //
        // public virtual List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();

        [JsonProperty("attendees")]
        public ICollection<AttendeeResponse> UserProperties { get; set; }
        
        [JsonProperty("activities")]
        public ICollection<ActivityResponse> Activities { get; set; }
        
    }
}