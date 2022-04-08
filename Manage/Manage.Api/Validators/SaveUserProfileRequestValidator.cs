using FluentValidation;
using Manage.API.Models.v1.Requests.Users;

namespace Manage.API.Validators
{
    public class SaveUserProfileRequestValidator: AbstractValidator<UserProfileRequest>
    {
        public SaveUserProfileRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 30);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 30);
            
            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .Length(3, 30);

            RuleFor(x => x.MobileCountryCode)
                .NotEmpty();

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .Length(9, 11);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}