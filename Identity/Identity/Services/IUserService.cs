using System.Threading.Tasks;
using Identity.API.Models.v1.Requests;
using Identity.API.Models.v1.Responses;

namespace Identity.API.Services
{
    public interface IUserService
    {
        
        Task<MeResponse> Me();
        
        Task<AuthResponse> Login(UserLoginRequest request);
        
        Task<bool> Register(UserRegisterRequest request);
        
        Task<AuthResponse> RefreshToken(RefreshTokenRequest request);
        
        Task<ConfirmEmailResponse> ConfirmEmail(ConfirmEmailRequest request);

        Task<UserManagerResponse> ForgotPassword(ForgotPasswordRequest request);
        
        Task<UserManagerResponse> ResetPassword(ResetPasswordRequest request);
    }
}