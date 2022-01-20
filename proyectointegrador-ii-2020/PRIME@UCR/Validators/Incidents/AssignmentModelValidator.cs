using FluentValidation;
using PRIME_UCR.Application.DTOs.Incidents;

namespace PRIME_UCR.Validators.Incidents
{
    public class AssignmentModelValidator : AbstractValidator<AssignmentModel>
    {
        public AssignmentModelValidator()
        {
            // RuleFor(a => a.Coordinator)
            //     .NotEmpty()
            //     .WithMessage("Debe seleccionar un coordinador");
            //
            // RuleFor(a => a.TeamMembers)
            //     .NotEmpty()
            //     .WithMessage("Debe seleccionar al menos un miembro del equipo");
            //
            // RuleFor(a => a.TransportUnit)
            //     .NotEmpty()
            //     .WithMessage("Debe seleccionar una unidad de transporte");
        }
    }
}