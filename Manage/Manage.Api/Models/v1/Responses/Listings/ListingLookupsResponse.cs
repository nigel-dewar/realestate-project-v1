using System.Collections.Generic;
using Manage.Repository.Entities;

namespace Manage.API.Models.v1.Responses.Listings
{
    public class ListingLookupsResponse
    {
        public IEnumerable<PropertyType> PropertyTypes { get; set; }
        public IEnumerable<Feature> Features { get; set; }
    }
}