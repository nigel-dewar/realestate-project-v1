using System.Collections.Generic;

namespace Manage.Repository.Entities
{
    public class UserType
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public ICollection<UserProfileType> UserProfileTypes { get; set; } = new List<UserProfileType>();

    }
}