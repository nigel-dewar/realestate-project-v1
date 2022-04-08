namespace Manage.API.Models.v1.Requests.Listings
{
    public class CreateListingRequest
    {
        public string PropertyId { get; set; }

        public long DateListingStarts { get; set; }

        public string ContactEmail { get; set; } // used to send emails to lister

        public string ContactNumber { get; set; } // can be landline or mobile

        public bool ShowNumber { get; set; }

        // Asking price
        public decimal Price { get; set; } 

        public string ListingType { get; set; } // rent, buy, auction etc
        
        
    }
}