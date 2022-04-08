using System;
using System.ComponentModel.DataAnnotations;

namespace Manage.Repository.Entities
{
    public class ListingAction
    {
        [Key]
        public int Id { get; set; }

        public string ListingId { get; set; }

        public string Action { get; set; }

        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        public string FulfillmentId { get; set; }

        public DateTimeOffset DateFulfilled { get; set; }
        
    }
}