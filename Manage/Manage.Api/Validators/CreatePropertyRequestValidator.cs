using FluentValidation;
using Manage.API.Models.v1.Requests.Properties;

namespace Manage.API.Validators
{
    public class CreatePropertyRequestValidator: AbstractValidator<CreatePropertyRequest>
    {
        public CreatePropertyRequestValidator()
        {

            RuleFor(x => x.PropertyTypeId)
                .NotEmpty();
            
            RuleFor(x => x.Bedrooms)
                .NotEmpty();
            
            RuleFor(x => x.Bathrooms)
                .NotEmpty();

            RuleFor(x => x.ParkingSpaces)
                .NotEmpty();

            RuleFor(x => x.StreetName)
                .NotEmpty();
            
            RuleFor(x => x.StreetNumber)
                .NotEmpty();
            
            RuleFor(x => x.PostalCode)
                .NotEmpty();

            RuleFor(x => x.RegionOrState)
                .NotEmpty();
            
            RuleFor(x => x.Country)
                .NotEmpty();
            
            RuleFor(x => x.Latitude)
                .NotEmpty();
            
            RuleFor(x => x.Longitude)
                .NotEmpty();
            
            RuleFor(x => x.GoogleAddress)
                .NotEmpty();
        }
    }
}