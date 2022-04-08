using System.Collections.Generic;
using Manage.Repository.Entities;

namespace Manage.API.Models.v1.Responses.Filters
{
    public class FiltersResponse
    {
        public IEnumerable<PropertyType> PropertyTypes { get; set; }
        public IEnumerable<Feature> Features { get; set; }
    }
}
