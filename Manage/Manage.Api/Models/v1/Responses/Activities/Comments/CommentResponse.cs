using System;

namespace Manage.API.Models.v1.Responses.Activities.Comments
{
    public class CommentResponse
    {
        public Guid Id { get; set; }

        public string Body { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Image { get; set; }
    }
}