using System.Collections.Generic;

namespace Manage.Repository.Entities
{
    public class PropertyType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public List<Property> Properties { get; set; } = new List<Property>();
    }
}
