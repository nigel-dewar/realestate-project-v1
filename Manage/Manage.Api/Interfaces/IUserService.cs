using System.Collections.Generic;
using System.Threading.Tasks;
using Manage.API.Models.v1.Requests.Users;
using Manage.API.Models.v1.Responses.Users;

namespace Manage.API.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileResponse> GetUserProfile();
        
        Task<UserProfileResponse> SaveUserProfile(UserProfileRequest request);
        
        Task<List<UserTypeResponse>> GetUserTypes();

        Task<UserMobileCheckResponse> CheckMobileNumber(UserMobileCheckRequest request);
        Task<bool> SubmitMobileCode(UserMobileCodeRequest request);
    }
}