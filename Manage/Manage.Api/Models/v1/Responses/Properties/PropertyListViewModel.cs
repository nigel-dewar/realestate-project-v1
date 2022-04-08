using Manage.API.Models.v1.Responses.Listings;

namespace Manage.API.Models.v1.Responses.Properties
{
    public class PropertyListViewModel
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Suburb { get; set; }

        public string SuburbSlug { get; set; }

        public string State { get; set; }

        public string PostCode { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public int ParkingSpaces { get; set; }

        public decimal LandSize { get; set; }

        public ListingResponse Listing { get; set; }

        //public virtual ICollection<Image> Images { get; set; }
    }
}
