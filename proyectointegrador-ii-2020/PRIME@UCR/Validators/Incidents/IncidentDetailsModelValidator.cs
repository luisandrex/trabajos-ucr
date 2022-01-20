using FluentValidation;
using PRIME_UCR.Application.Dtos.Incidents;

namespace PRIME_UCR.Validators.Incidents
{
    public class IncidentDetailsModelValidator : AbstractValidator<IncidentDetailsModel>
    {
        public IncidentDetailsModelValidator()
        {
            RuleFor(i => i.Origin)
                .NotEmpty()
                .WithMessage("Debe seleccionar un origen.");
            
            RuleFor(i => i.Destination)
                .NotEmpty()
                .WithMessage("Debe seleccionar un destino.");
        }
    }
}