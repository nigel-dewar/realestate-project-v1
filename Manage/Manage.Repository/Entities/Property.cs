using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manage.Repository.Entities
{
    public class Property
    {
        [Key]
        public string Id { get; set; }
        public string CreatedById { get; set; }
        
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        public string Name { get; set; }
        public int AddressChanged { get; set; } = 0;

        public bool CanEditAddress { get; set; } = true;
        public string Slug { get; set; } // Generated on creation
        public string SuburbSlug { get; set; } // Generated on creation
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        public string Description { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int ParkingSpaces { get; set; }
        public int LandSize { get; set; } = 0;
        
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

        public string Address { get; set; }

        // Fields for Search functions. Filled out during Listing process
        
        public bool IsActive { get; set; } = false; 
        public bool IsForSale { get; set; } = false;
        public bool IsForRent { get; set; } = false;
        public bool IsForAuction { get; set; } = false;
        public decimal BuyPrice { get; set; }
        public decimal RentPrice { get; set; }
        public DateTimeOffset AvailableDate { get; set; }
        
        // Associated Collections
        
        public ICollection<PropertyFeature> PropertyFeatures { get; set; }
        public ICollection<Listing> Listings { get; set; }
        
        public ICollection<Image> Images { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
        public ICollection<UserProperty> UserProperties { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}