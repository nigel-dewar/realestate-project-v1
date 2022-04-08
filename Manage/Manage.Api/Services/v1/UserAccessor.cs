using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Manage.API.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Manage.API.Services.v1
{
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
        
        public string GetUserSub()
        {
            
            var sub = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(
                x=>x.Type == "sub")?.Value;

            return sub;
        }
        
        public List<Claim> GetUserClaims()
        {

            // var returnList = new List<string>();
            var claims = _httpContextAccessor.HttpContext.User?.Claims?.ToList();

            // foreach (var claim in claims)
            // {
            //     returnList.Add(claim.Value);
            // }

            return claims;
        }
    }
}