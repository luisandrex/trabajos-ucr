using FluentValidation;
using PRIME_UCR.Application.Dtos.Incidents;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Validators.Incidents
{
    public class HouseholdModelValidator : AbstractValidator<HouseholdModel>
    {
        public HouseholdModelValidator()
        {
            RuleFor(h => h.Address)
                .NotEmpty()
                .WithMessage("Debe digitar una dirección.");
            
            RuleFor(h => h.Address)
                .MaximumLength(150)
                .WithMessage("La dirección exacta no puede exceder 150 caracteres.");

            RuleFor(h => h.Province)
                .NotEmpty()
                .WithMessage("Debe escoger una provincia.");

            RuleFor(h => h.Canton)
                .NotEmpty()
                .WithMessage("Debe escoger un cantón.");

            RuleFor(h => h.District)
                .NotEmpty()
                .WithMessage("Debe escoger un distrito.");

            RuleFor(h => h.Longitude)
                .NotEmpty()
                .WithMessage("Debe digitar la longitud.");

            RuleFor(h => h.Latitude)
                .NotEmpty()
                .WithMessage("Debe digitar la latidud.");
        }
    }
}