using System.ComponentModel.DataAnnotations;

namespace Manage.Repository.Entities
{
    public class PostCode
    {

        [Required]
        [Key]
        public string Code { get; set; }
        public string Locality { get; set; }
        public string State { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }
        public string DeliveryCentre { get; set; }
        public string Type { get; set; }
        
        public string Status { get; set; }
        public string Suburb { get; set; }
    }
}
