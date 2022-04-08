using System;

namespace Manage.Repository.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public UserProfile Author { get; set; }

        public Activity Activity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}