using FluentValidation;
using PRIME_UCR.Application.Dtos.Incidents;

namespace PRIME_UCR.Validators.Incidents
{
    public class MedicalCenterLocationModelValidator : AbstractValidator<MedicalCenterLocationModel>
    {
        public MedicalCenterLocationModelValidator()
        {
            RuleFor(m => m.Doctor)
                .NotEmpty()
                .WithMessage("Debe seleccionar un/una médico/médica.");

            RuleFor(m => m.BedNumber)
                .Custom((i, context) =>
                {
                    var model = (MedicalCenterLocationModel) context.InstanceToValidate;
                    // only validate for origin
                    if (model.IsOrigin)
                    {
                        if (i == null)
                        {
                            context.AddFailure("Debe seleccionar un número de cama.");
                        }
                        else if (i <= 0)
                        {
                            context.AddFailure("El número de cama debe ser mayor a 0.");
                        }
                    }
                });
        }
    }
}