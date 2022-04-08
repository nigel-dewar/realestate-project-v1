using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manage.Repository.Entities
{
    public class Agent
    {
        [Key]
        public string Id { get; set; }

        public string UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }

        public string LicenceNumber { get; set; } // all agents in AU must have this

        public string AgencyId { get; set; } // doesnt not have to belong to an agency

        public Agency Agency { get; set; } // does not have to belong to an agency

        public List<Listing> Listings { get; set; }

        public List<Property> Properties { get; set; }

    }
}
