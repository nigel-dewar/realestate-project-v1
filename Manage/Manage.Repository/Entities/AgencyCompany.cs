using System.Collections.Generic;

namespace Manage.Repository.Entities
{
    public class AgencyCompany
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string LogoImageUrl { get; set; }

        public string BrandColor { get; set; }

        public  List<Agency> AgencyOffices { get; set; } = new List<Agency>();

    }
}
