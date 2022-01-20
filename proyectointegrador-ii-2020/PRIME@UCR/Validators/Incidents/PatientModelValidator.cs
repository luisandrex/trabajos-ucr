using System;
using FluentValidation;
using PRIME_UCR.Application.Dtos.Incidents;

namespace PRIME_UCR.Validators.Incidents
{
    public class PatientModelValidator : AbstractValidator<PatientModel>
    {
        public PatientModelValidator()
        {
            RuleFor(i => i.CedPaciente)
                .NotEmpty()
                .WithMessage("Debe digitar una cédula.")
                .DependentRules(() =>
                {
                    // TODO: change length to 9 after updating post deployment
                    RuleFor(i => i.CedPaciente)
                        .Length(9)
                        .WithMessage("Debe digitar una cédula válida (9 números).")
                        .DependentRules(() =>
                        {
                            RuleFor(i => i.CedPaciente)
                                .Must(ced => Int32.TryParse(ced, out _))
                                .WithMessage("Debe digitar una cédula válida (8 números).");
                        });
                });
        }
    }
}