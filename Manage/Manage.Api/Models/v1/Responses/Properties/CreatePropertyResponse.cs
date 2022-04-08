using System;

namespace Manage.API.Models.v1.Responses.Properties
{
    public class CreatePropertyResponse
    {
        public string Id { get; set; }

        public string CreatedById { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public string Name { get; set; }
        
        public int AddressChanged { get; set; }
        
        public bool CanEditAddress { get; set; }
        
        public string Slug { get; set; }
        
        public string SuburbSlug { get; set; }

        public int PropertyTypeId { get; set; }
        
        public string Description { get; set; }

        public int Bedrooms { get; set; }
        
        public int Bathrooms { get; set; }
        
        public int ParkingSpaces { get; set; }
        
        public decimal LandSize { get; set; }
        
        public string StreetNumber { get; set; }
        
        public string StreetName { get; set; }
        
        public string PostalCode { get; set; }
        
        public string SuburbOrCity { get; set; } 
        
        public string Council { get; set; } 
        
        public string RegionOrState { get; set; } 
        
        public string Country { get; set; }
        
        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public string GoogleAddress { get; set; }
    }
}