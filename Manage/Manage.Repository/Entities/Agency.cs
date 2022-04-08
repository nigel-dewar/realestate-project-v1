using System.Collections.Generic;

namespace Manage.Repository.Entities
{
    public class Agency
    {

        public string Id { get; set; }
        public string AgencyCompanyId { get; set; }
        public string AgencyCompanyName { get; set; }
        public string AgencyOfficeName { get; set; }
        public string Locality { get; set; }
        public string LogoImageUrl { get; set; }
        public string PostCode { get; set; }

        public AgencyCompany AgencyCompany { get; set; }
        
        public ICollection<Agent> Agents { get; set; } = new HashSet<Agent>();
        public ICollection<Listing> Listings { get; set; } = new HashSet<Listing>();
    }
}
