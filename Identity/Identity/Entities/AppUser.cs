using Microsoft.AspNetCore.Identity;

namespace Identity.API.Entities
{
    public class AppUser: IdentityUser
    {
        public string RefreshToken { get; set; }
    }
}