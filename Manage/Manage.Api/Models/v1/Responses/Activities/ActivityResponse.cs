using System;
using System.Collections.Generic;
using Manage.API.Models.v1.Responses.Activities.Comments;
using Newtonsoft.Json;

namespace Manage.API.Models.v1.Responses.Activities
{
    public class ActivityResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string City { get; set; }

        public string Venue { get; set; }

        public string ActivityType { get; set; }
        
        [JsonProperty("attendees")]
        public virtual ICollection<AttendeeResponse> UserActivities { get; set; }
        
        [JsonProperty("comments")]
        public virtual ICollection<CommentResponse> Comments { get; set; }
        
    }
}