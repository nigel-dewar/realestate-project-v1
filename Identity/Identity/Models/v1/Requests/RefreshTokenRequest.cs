using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.v1.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}