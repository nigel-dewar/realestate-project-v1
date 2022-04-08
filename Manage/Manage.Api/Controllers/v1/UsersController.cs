using System.Collections.Generic;
using System.Threading.Tasks;
using Manage.API.Interfaces;
using Manage.API.Models.v1.Requests.Users;
using Manage.API.Models.v1.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manage.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController
    {
        private readonly IUserService _usersService;
        
        public UsersController(IUserService usersService)
        {
            _usersService = usersService;
        }
        
        // Get User Profile
        // /api/v1/users
        [HttpGet()]
        public async Task<ActionResult<UserProfileResponse>> GetUserProfile()
        {
            return await _usersService.GetUserProfile();
        }
        
        // Get User Types
        // /api/v1/users/user-types
        [HttpGet("user-types")]
        public async Task<ActionResult<List<UserTypeResponse>>> GetUserTypes()
        {
            return await _usersService.GetUserTypes();
        }
        
        // Save or Update User Profile
        // /api/v1/users
        [HttpPost()]
        public async Task<ActionResult<UserProfileResponse>> SaveListing(UserProfileRequest request)
        {
            return await _usersService.SaveUserProfile(request);
        }
        
        // Check users Mobile number
        // /api/v1/users/check-mobile-number
        [HttpPost("check-mobile-number")]
        public async Task<ActionResult<UserMobileCheckResponse>> CheckMobileNumber(UserMobileCheckRequest request)
        {
            return await _usersService.CheckMobileNumber(request);
        }
        
        // Submit Mobile Code
        // /api/v1/listings
        [HttpPost("submit-mobile-code")]
        public async Task<ActionResult<bool>> SubmitMobileCode(UserMobileCodeRequest request)
        {
            return await _usersService.SubmitMobileCode(request);
        }
        
        
        
    }
}