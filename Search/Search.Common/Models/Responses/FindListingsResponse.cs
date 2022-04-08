using System.Collections.Generic;

namespace Search.Common.Models.Responses
{
    public class FindListingsResponse
    {
        public int Total { get; set; }

        public int CurrentQueryCount { get; set; }

        public int AvailablePages { get; set; }

        public int CurrentPageNumber { get; set; }


        public List<ListingModel> Properties { get; set; }
    }
}
