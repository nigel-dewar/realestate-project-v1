using FluentValidation;
using Identity.API.Models.v1.Requests;

namespace Identity.API.Validators
{
    public class UserRegisterRequestValidator: AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator()
        {
            // RuleFor(x => x.UserName)
            //     .NotEmpty()
            //     .Length(3, 15).Matches(@"\A\S+\z").WithMessage("Please ensure that your UserName is more between 3 and 15 characters, only contains numbers and letters and does not contain any spaces.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
            
            
        }
    }
}