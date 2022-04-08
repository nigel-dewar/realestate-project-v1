using FluentValidation;
using Manage.API.Models.v1.Requests.Users;

namespace Manage.API.Validators
{
    public class UserMobileCheckRequestValidator: AbstractValidator<UserMobileCheckRequest>
    {
        public UserMobileCheckRequestValidator()
        {
            RuleFor(x => x.CountryCode)
                .NotEmpty();
            
            RuleFor(x => x.MobileNumber)
                .NotEmpty();
        }
    }
}