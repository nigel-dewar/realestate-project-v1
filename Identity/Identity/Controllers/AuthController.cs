using System;
using System.Threading.Tasks;
using Identity.API.Models.v1.Requests;
using Identity.API.Models.v1.Responses;
using Identity.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        private IMailService _mailService;
        private IConfiguration _configuration;
        private readonly ILogger _logger;

        public AuthController(IUserService userService, IMailService mailService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userService = userService;
            _mailService = mailService;
            _configuration = configuration;
            _logger = logger;
        }
        
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<MeResponse>> Me()
        {
            try
            {
                // var user = HttpContext.User;
                var res = await _userService.Me();
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
 
        }
        
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Login(UserLoginRequest request)
        {
            return await _userService.Login(request);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<bool>> Register(UserRegisterRequest request)
        {
            return await _userService.Register(request);
        }
        
        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return NotFound();
            }
            
            var request = new ConfirmEmailRequest
            {
                UserId = userId,
                Token = token
            };
            
            try
            {
                var res = await _userService.ConfirmEmail(request);

                return res;
                // if (res.IsSuccess)
                // {
                //     return Redirect($"{_configuration["EmailSettings:ReplyUrl"]}/ConfirmedEmail.html");
                // }

                return BadRequest(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<UserManagerResponse>> ForgotPassword(ForgotPasswordRequest request)
        {
            return await _userService.ForgotPassword(request);
        }
        
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<UserManagerResponse>> ResetPassword(ResetPasswordRequest request)
        {
            return await _userService.ResetPassword(request);
        }
        
        // [AllowAnonymous]
        // [HttpGet("ConfirmEmail")]
        // public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail(ConfirmEmailRequest request)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest();
        //     
        //     try
        //     {
        //         var res = await _userService.ConfirmEmail(request);
        //         if (res.IsSuccess)
        //         {
        //             return Redirect($"{_configuration["EmailSettings:ReplyUrl"]}/ConfirmedEmail.html");
        //         }
        //
        //         return BadRequest(res);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         return BadRequest(e.Message);
        //     }
        // }

        [HttpPost("Refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Refresh(RefreshTokenRequest request)
        {
            return await _userService.RefreshToken(request);
        }
    }
}