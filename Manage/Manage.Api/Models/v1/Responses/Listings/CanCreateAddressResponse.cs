namespace Manage.API.Models.v1.Responses.Listings
{
    public class CanCreateAddressResponse
    {
        public bool CanCreate { get; set; }

        public string Message { get; set; }

        public bool CreatedByUser { get; set; }

        public bool HasListing { get; set; }

        public bool IsActiveListing { get; set; }
        
    }
}