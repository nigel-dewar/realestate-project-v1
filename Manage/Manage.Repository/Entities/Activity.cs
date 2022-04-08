using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manage.Repository.Entities
{
    public class Activity
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string City { get; set; }

        public string Venue { get; set; }

        public string ActivityType { get; set; }
        
        public ICollection<UserActivity> UserActivities { get; set; }
        
        public ICollection<Comment> Comments { get; set; }

    }
}