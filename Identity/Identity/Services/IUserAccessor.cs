using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Identity.API.Services
{
    public interface IUserAccessor
    {
        string GetCurrentUsername();
        string GetCurrentUserId();
    }
    
    public class UserAccessor: IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GetCurrentUsername()
        {

            var username = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(
                x=>x.Type == ClaimTypes.Name)?.Value;

            return username;
        }

        public string GetCurrentUserId()
        {
            
            var userId = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(
                    x=>x.Type == ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}