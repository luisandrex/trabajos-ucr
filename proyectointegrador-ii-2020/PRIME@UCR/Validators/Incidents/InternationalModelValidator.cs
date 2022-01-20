using FluentValidation;
using PRIME_UCR.Application.Dtos.Incidents;

namespace PRIME_UCR.Validators.Incidents
{
    public class InternationalModelValidator : AbstractValidator<InternationalModel>
    {
        public InternationalModelValidator()
        {
            RuleFor(i => i.Country)
                .NotEmpty()
                .WithMessage("Debe seleccionar un país.");
        }
    }
}