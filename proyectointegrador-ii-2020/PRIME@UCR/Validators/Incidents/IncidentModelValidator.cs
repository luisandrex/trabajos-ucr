using System;
using FluentValidation;
using FluentValidation.Validators;
using PRIME_UCR.Application.Dtos.Incidents;

namespace PRIME_UCR.Validators.Incidents
{
    public class IncidentModelValidator : AbstractValidator<IncidentModel>
    {
        public IncidentModelValidator()
        {
            RuleFor(i => i.Mode)
                .NotEmpty()
                .WithMessage("Debe seleccionar una modalidad");
            RuleFor(i => i.EstimatedDateOfTransfer)
                .NotEmpty()
                .WithMessage("Debe seleccionar una fecha estimada de traslado")
                .DependentRules(() =>
                {
                    RuleFor(i => i.EstimatedDateOfTransfer)
                        .Must(IsFutureDate)
                        .WithMessage("Debe seleccionar una fecha en el futuro.");
                });
        }

        private bool IsFutureDate(DateTime? date)
        {
            return date?.CompareTo(DateTime.Now) > 0;
        }
    }
}