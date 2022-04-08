﻿namespace Manage.Repository.Entities
{
    public class Image
    {
        public string Id { get; set; }

        public string Url { get; set; }
        
        public bool IsMain { get; set; } = false;
        
    }
}
