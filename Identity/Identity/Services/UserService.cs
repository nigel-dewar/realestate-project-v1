using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Identity.API.Data;
using Identity.API.Entities;
using Identity.API.Helpers;
using Identity.API.Models;
using Identity.API.Models.v1.Requests;
using Identity.API.Models.v1.Responses;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Constants;
using MessageBus.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Identity.API.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        // private readonly IJwtGenerator _jwtGenerator;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger _logger;
        private readonly IBusControl _busControl;
        
        private IMailService _mailService;

        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            DataContext context,
            IConfiguration configuration,
            IMailService mailService,
            IUserAccessor userAccessor,
            ILogger logger, IBusControl busControl)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            // _jwtGenerator = jwtGenerator;
            _context = context;
            _configuration = configuration;
            _mailService = mailService;
            _userAccessor = userAccessor;
            _logger = logger;
            _busControl = busControl;
        }

        public async Task<MeResponse> Me()
        {
            var userId = _userAccessor.GetCurrentUserId();

            // var user = _userManager.FindByIdAsync(userId);
            var user = await _userManager.Users.SingleAsync(x=>x.Id == userId);
            
            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, "Could not verify user");
            
            // var token = await GenerateAuthResponse(user);
            return new MeResponse
            {
                Username = user.UserName,
                Email = user.Email
            };

        }

        public async Task<AuthResponse> Login(UserLoginRequest request)
        {

            
            var user = await _userManager.Users.SingleOrDefaultAsync(x=>x.NormalizedEmail == request.Email);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = "Incorrect username or password", code = "555"} );
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                var token = await GenerateAuthResponse(user);
                return token;
            }

            if (result.IsLockedOut)
            {
                throw new RestException(HttpStatusCode.Locked, new {message = "Account is locked out", code = "557"});
            }

            if (user.EmailConfirmed != true)
            {
                throw new RestException(HttpStatusCode.Unauthorized, new {message = "Account not confirmed. Please check your email for confirmation of your account.", code ="556"});
            }
            

            throw new RestException(HttpStatusCode.BadRequest, new {message = "Bad Request", code = "404"});
        }
        
        public async Task<bool> Register(UserRegisterRequest request)
        {
            _logger.Information($"Beginning User registration with Email: {request.Email} and UserName: {request.UserName}");
            if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
            {
                _logger.Error($"User attempted email registration with Email: {request.Email}. This email already exists");
                throw new RestException(HttpStatusCode.BadRequest, new {message = "Email already exists", code = 0});
            }

            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                LockoutEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new RestException(HttpStatusCode.BadRequest, new {message = "Could not register user", code = 0});

            await _userManager.AddToRoleAsync(user, "User");

            var correlationId = Guid.NewGuid().ToString();
            _logger.Information($"Sending Registration Confirmation Email to: {request.Email} with CorrelationId: {correlationId}");
            
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            var url = $"{_configuration["AppSettings:FrontEndUrl"]}/verification/verify-account?userId={user.Id}&token={validEmailToken}";

            // await _mailService.SendEmailAsync(user.Email, "Confirm your email", $"<h1>Welcome to Realestateify.</h1> <p>confirm your new account here </p><br><br> <a href='{url}'>Click here</a>");

            try
            {
                var rabbitUri = $"rabbitmq://{_configuration["EventBus:HostName"]}/";
                var sentToUri = new Uri(rabbitUri + $"{EventBusConstants.EmailGenericCommand}");

                var endPoint = await _busControl.GetSendEndpoint(sentToUri);
                await endPoint.Send<IEmailGenericCommand>(
                    new EmailGenericCommand()
                    {
                        CorrelationId = correlationId,
                        ToEmail = user.Email,
                        ToName = user.Email,
                        Subject = "Please Confirm your Realestateify Account",
                        Message = $"<h1>Welcome to Realestateify.</h1> <br><br><p>confirm your new account here </p><br><br> <a href='{url}'>here</a>"
                    }
                );
            }
            catch (Exception e)
            {
                _logger.Error($"Could not setup MessageBus to send Confirmation Email");
                throw;
            }
            
            _logger.Information($"User Registration Successful with Email: {request.Email} and UserName: {request.UserName}");
            
            return true;

        }
        
        public async Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
        {
            
            var user = await _userManager.Users.SingleOrDefaultAsync(x=> x.RefreshToken == request.RefreshToken);

            if (user == null)
            {
                throw new Exception("User not found for token");
            }

            var token = await GenerateAuthResponse(user);
            return token;

        }

        public async Task<ConfirmEmailResponse> ConfirmEmail(ConfirmEmailRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return new ConfirmEmailResponse
                    {
                        IsSuccess = false,
                        Message = "User Not found"
                    };
                }

                var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
                string normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManager.ConfirmEmailAsync(user, normalToken);

                // Send Welcome email to user
                try
                {
                    var rabbitUri = $"rabbitmq://{_configuration["EventBus:HostName"]}/";
                    var sentToUri = new Uri(rabbitUri + $"{EventBusConstants.EmailGenericCommand}");

                    var endPoint = await _busControl.GetSendEndpoint(sentToUri);
                    await endPoint.Send<IEmailGenericCommand>(
                        new EmailGenericCommand()
                        {
                            CorrelationId = Guid.NewGuid().ToString(),
                            ToEmail = user.Email,
                            Message = $"<h1>Welcome to Realestateify</h1>  <br><br> <p>Please Signin at <a href='{_configuration["AppSettings:FrontEndUrl"]}/login'>Click here</a></p>",
                            Subject = "Welcome to Realestateify",
                            ToName = user.Email
                        }
                    );
                }
                catch (Exception e)
                {
                    _logger.Error($"Could not setup MessageBus to send Welcome Email");
                    throw;
                }
                
                if (result.Succeeded)
                {
                    return new ConfirmEmailResponse
                    {
                        IsSuccess = true,
                        Message = "Email confirmed Successfully"
                    };
                    
                }

                return new ConfirmEmailResponse
                {
                    IsSuccess = false,
                    Message = "Email did not confirm",
                    Errors = result.Errors.Select(e=>e.Description)
                };

            }
            catch (Exception e)
            {
                _logger.Error(e,$"Confirm email process failed");
                throw;
            }
        }

        public async Task<UserManagerResponse> ForgotPassword(ForgotPasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "No user associated with email"
                    };

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                
                var encodedEmailToken = Encoding.UTF8.GetBytes(token);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                var url = $"{_configuration["AppSettings:FrontEndUrl"]}/password/reset?userId={user.Id}&token={validEmailToken}";

                await _mailService.SendEmailAsync(user.Email, "Reset Password", $"<p>Reset your password here </p><br><br> <a href='{url}'>Click here</a>");

                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "Password reset sent successfully"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserManagerResponse> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                    return new UserManagerResponse
                    {
                        IsSuccess = true,
                        Message = "No user associated with this email"
                    };

                if (request.Password != request.PasswordConfirmation)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Password does not match its confirmation"
                    };
                }
                
                var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
                string normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManager.ResetPasswordAsync(user, normalToken, request.Password);

                if (result.Succeeded)
                {
                    var url = $"{_configuration["AppSettings:FrontEndUrl"]}/login";
                    await _mailService.SendEmailAsync(user.Email, "Your password was successfully reset", $"<p>Go ahead and log back in! </p><br><br> <a href='{url}'>Click here</a>");
                    return new UserManagerResponse
                    {
                        IsSuccess = true,
                        Message = "Password has been successfully reset"
                    };
                }

                

                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Password reset failed",
                    Errors = result.Errors.Select(x=>x.Description)
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<AuthResponse> GenerateAuthResponse(AppUser user)
        {
            var claims = new List<Claim>
            {
                // new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                // new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // new Claim(ClaimTypes.Name, user.UserName),
                // new Claim("aud",  _configuration["Authentication:Audience"]),
                new Claim("aud",  "http://localhost:5001"),
                new Claim("aud",  "http://localhost:5002"),
                new Claim("aud",  "http://localhost:5003"),
                new Claim("aud",  "http://localhost:5501"),
                new Claim("aud",  "http://localhost:5502"),
                new Claim("aud",  "http://localhost:5503"),
                // new Claim("aud",  "http://192.168.1.50:3000"),
            };
            
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expires = DateTime.Now.AddSeconds(Convert.ToDouble(_configuration["Authentication:JwtExpireMins"]));

            // expires in a value is seconds. 60 mins = 3600 seconds
            // var expiresIn = (Convert.ToInt16(_configuration["Authentication:JwtExpireMins"]) * 60);
            var expiresIn = (Convert.ToInt16(_configuration["Authentication:JwtExpireMins"]));
    
            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Subject = new ClaimsIdentity(claims),
            //     Expires = expires,
            //     SigningCredentials = creds,
            // };
            //
            // var tokenHandler = new JwtSecurityTokenHandler();
            //
            // var tokenCreated = tokenHandler.CreateToken(tokenDescriptor);
            //
            // var token = tokenHandler.WriteToken(tokenCreated);
            
            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            
            // Create a refresh token and save against user in db
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
            
            return new AuthResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Roles = roles,
                AccessTokenExpiration = expires,
                ExpiresIn = expiresIn,
                Email = user.Email
            };
            
        }


    }
}