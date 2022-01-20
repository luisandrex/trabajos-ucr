using FluentValidation;
using PRIME_UCR.Domain.Models.CheckLists;

namespace PRIME_UCR.Validators.CheckLists
{
    public class CheckListValidator : AbstractValidator<CheckList>
    {
        public CheckListValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty()
                .WithMessage("Debe ingresar un nombre.")
                .MaximumLength(200)
                .WithMessage("El nombre no puede contener más de 200 carácteres.");
            RuleFor(c => c.Descripcion)
                .MaximumLength(500)
                .WithMessage("La descripción no puede contener más de 500 cáracteres");
            RuleFor(c => c.Tipo)
                .NotEmpty()
                .WithMessage("Debe seleccionar un tipo.");
            RuleFor(c => c.Orden)
                .NotEmpty()
                .WithMessage("Debe ingresar un orden.")
                .GreaterThanOrEqualTo(1)
                .WithMessage("Debe ingresar un orden mayor o igual que 1");
        }
    }
}
