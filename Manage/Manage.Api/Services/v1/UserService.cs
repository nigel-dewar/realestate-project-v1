using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Manage.API.Helpers;
using Manage.API.Interfaces;
using Manage.API.Models;
using Manage.API.Models.v1.Requests.Users;
using Manage.API.Models.v1.Responses.Users;
using Manage.Repository.Data;
using Manage.Repository.Entities;
using MassTransit;
using MessageBus.Commands;
using MessageBus.Constants;
using MessageBus.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Manage.API.Services.v1
{
    public class UserService: IUserService
    {

        private readonly ManageDataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly IImageService _imageService;
        private readonly ILogger _logger;
        private readonly IBusControl _busControl;
        private readonly IConfiguration _configuration;


        public UserService(ManageDataContext context, IMapper mapper, IUserAccessor userAccessor, IImageService imageService, ILogger logger, IBusControl busControl, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _imageService = imageService;
            _logger = logger;
            _busControl = busControl;
            _configuration = configuration;
        }
        
        public async Task<UserProfileResponse> GetUserProfile()
        {
            try
            {
                var userId = _userAccessor.GetCurrentUserId();

                var profile = await _context.Users.Include(x=>x.UserProfileTypes).Where(x => x.Id == userId).SingleOrDefaultAsync();

                if (profile == null)
                {
                    var newUserProfile = new UserProfile
                    {
                        Id = userId,
                        DateCreated = DateTimeOffset.UtcNow
                    };
                    
                    _context.Users.Add(newUserProfile);
                    
                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                    {
                        return new UserProfileResponse
                        {
                            Id = userId
                        };
                    }
                    
                    throw new RestException(HttpStatusCode.BadGateway, new { Errors = "Could not add user profile to database"});
                }
                
                var userTypes = profile.UserProfileTypes.Select(x=> new UserTypeResponse
                {
                    Id = x.UserTypeId
                }).ToList();

                return new UserProfileResponse
                {
                    Id = profile.Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    DisplayName = profile.DisplayName,
                    Email = profile.Email,
                    MobileNumber = profile.MobileNumber,
                    MobileCountryCode = profile.MobileCountryCode,
                    MobileConfirmed = profile.MobileConfirmed,
                    Bio = profile.Bio,
                    UserProfileComplete = profile.UserProfileComplete,
                    UserTypes = userTypes,
                    DateCreated = profile.DateCreated
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserProfileResponse> SaveUserProfile(UserProfileRequest request)
        {
            try
            {
                var userId = _userAccessor.GetCurrentUserId();

                var profile = await _context.Users.Include(x=>x.UserProfileTypes).Where(x => x.Id == userId).SingleOrDefaultAsync();
                
                if (profile == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Errors = "No user profile exists for this user"});

                profile.FirstName = request.FirstName;
                profile.LastName = request.LastName;
                profile.DisplayName = request.DisplayName;
                profile.Email = request.Email;
                profile.MobileCountryCode = request.MobileCountryCode;
                profile.MobileNumber = request.MobileNumber;
                profile.Bio = request.Bio;
                profile.FirstName = request.FirstName;

                var userTypes = new List<UserTypeResponse>();
                
                if (request.UserTypes.Count > 0)
                {
                    foreach (var type in profile.UserProfileTypes.ToList())
                    {
                        profile.UserProfileTypes.Remove(type);
                    }
                    
                    foreach (var userType in request.UserTypes)
                    {
                        var type = await _context.UserTypes.Where(b => b.Id == userType.Id).SingleAsync();
                        profile.UserProfileTypes.Add(new UserProfileType()
                        {
                            UserProfileId = userId,
                            UserTypeId = userType.Id
                        });
                        userTypes.Add(new UserTypeResponse
                        {
                            Id = type.Id
                        });
                    }
                }

                
                
                // check completion status
                var complete = CheckProfileComplete(profile);

                if (complete == true)
                {
                    profile.UserProfileComplete = true;
                }


                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new RestException(HttpStatusCode.BadRequest, new { Errors = "Unable to save user profile in database"} );
                }

                return new UserProfileResponse
                {
                    Id = profile.Id,
                    DateCreated = profile.DateCreated,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    DisplayName = profile.DisplayName,
                    Email = profile.Email,
                    MobileCountryCode = profile.MobileCountryCode,
                    MobileNumber = profile.MobileNumber,
                    MobileConfirmed = profile.MobileConfirmed,
                    Bio = profile.Bio,
                    UserProfileComplete = profile.UserProfileComplete,
                    UserTypes = userTypes
                };
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private bool CheckProfileComplete(UserProfile profile)
        {

            if (profile.MobileConfirmed == false)
                return false;

            return true;
        }

        public async Task<List<UserTypeResponse>> GetUserTypes()
        {
            try
            {
                var userTypes = await _context.UserTypes
                    .Select(x=> new UserTypeResponse
                        {
                            Id = x.Id,
                            Name = x.Name,
                        })
                        .ToListAsync();

                return userTypes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserMobileCheckResponse> CheckMobileNumber(UserMobileCheckRequest request)
        {
            try
            {
                var userId = _userAccessor.GetCurrentUserId();

                var profile = await _context.Users.Include(x=>x.UserProfileTypes).Where(x => x.Id == userId).SingleOrDefaultAsync();
                
                if (profile == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Errors = "No user profile exists for this user"});
                
                // block a dodgy user trying to trip the system

                if (profile.MobileConfirmationCodeFailedAttempts > 5)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, new { Errors = "You have exceeded the number of check mobile requests"} );
                }

                // save the profile confirmation code for later to validate
                var codeGen = new CodeGenerator();
                var mobileCode = codeGen.PhoneCode(4);
                profile.MobileConfirmationCode = mobileCode;
                profile.MobileConfirmationCodeSet = DateTimeOffset.UtcNow;
                
                // trim the mobile number
                string trimmedNumber = String.Concat(request.MobileNumber.Where(c => !Char.IsWhiteSpace(c)));
                profile.MobileNumber = trimmedNumber;
                profile.MobileCountryCode = request.CountryCode;
                

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error Storing users phone number.");
                    throw new RestException(HttpStatusCode.InternalServerError, new { Errors = "Error in storing phone number"} );
                }
                
                // TODO: REPLACE WITH CALL TO SMS SERVICE
                var rabbitUri = $"rabbitmq://{_configuration["EventBus:HostName"]}/";
                var sentToUri =
                    new Uri(rabbitUri + $"{EventBusConstants.SmsVerificationCommandQueue}");
                var client = _busControl.CreateRequestClient<ISmsVerificationCommand>(sentToUri);

                var response = await client.GetResponse<ISmsVerificationCommandResult>(new
                {
                    Number = profile.MobileCountryCode + profile.MobileNumber,
                    Code = mobileCode
                });

                if (!response.Message.SmsSent)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, new { Errors = "Error sending access code"} );
                }
                //var success = _smsService.SendMessage(profile.MobileCountryCode + profile.MobileNumber, mobileCode);

                // if (!response)
                //     throw new RestException(HttpStatusCode.InternalServerError, new { Errors = "Error sending access code"} );

                return new UserMobileCheckResponse
                {
                    MobileNumber = profile.MobileNumber,
                    CountryCode = profile.MobileCountryCode
                };


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> SubmitMobileCode(UserMobileCodeRequest request)
        {
            try
            {

                bool result = false;
                var userId = _userAccessor.GetCurrentUserId();

                var profile = await _context.Users.Include(x=>x.UserProfileTypes).Where(x => x.Id == userId).SingleOrDefaultAsync();
                
                if (profile == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Errors = "No user profile exists for this user"});
                
                if (profile.MobileConfirmationCodeFailedAttempts > 5)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, new { Errors = "You have exceeded the number of check mobile requests"} );
                }
                
                // check if incoming code matches the one we saved against the user
                if (profile.MobileConfirmationCode == request.Code.ToString())
                {
                    profile.MobileConfirmed = true;
                    profile.DateTimeMobileConfirmed = DateTimeOffset.UtcNow;
                    profile.MobileConfirmationCodeFailedAttempts = 0;
                    result = true;
                }
                else
                {
                    result = false;
                    profile.MobileConfirmationCodeFailedAttempts++;
                }
                
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new RestException(HttpStatusCode.InternalServerError, new { Errors = "Error confirming Code"} );
                }

                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}