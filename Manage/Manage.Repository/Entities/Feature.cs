using System.Collections.Generic;

namespace Manage.Repository.Entities
{
    public class Feature
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Outdoor { get; set; } = false;

        public List<PropertyFeature> ProductFeatures { get; set; } = new List<PropertyFeature>();
    }
}
