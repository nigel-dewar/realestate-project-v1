using System;

namespace Manage.Repository.Entities
{
    public class UserActivity
    {
        public string UserId { get; set; }

        public UserProfile UserProfile { get; set; }

        public Guid ActivityId { get; set; }

        public virtual Activity Activity { get; set; }

        public DateTime DateJoined { get; set; }

        public bool IsHost { get; set; }
    }
}