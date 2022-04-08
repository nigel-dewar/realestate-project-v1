
using System;

namespace Manage.Repository.Entities
{
    public class UserProperty
    {
        public string UserId { get; set; }

        public UserProfile UserProfile { get; set; }

        public string PropertyId { get; set; }

        public Property Property { get; set; }

        public DateTime DateJoined { get; set; }

        public bool IsHost { get; set; }


    }
}
