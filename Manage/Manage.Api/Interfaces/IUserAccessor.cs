using System.Collections.Generic;
using System.Security.Claims;

namespace Manage.API.Interfaces
{
    public interface IUserAccessor
    {
        string GetCurrentUsername();
        
        string GetCurrentUserId();
        
        string GetUserSub();

        List<Claim> GetUserClaims();
    }
}