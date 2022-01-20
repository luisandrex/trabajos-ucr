using FluentValidation;
using PRIME_UCR.Application.DTOs.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Validators.UserAdministration
{
    public class RegisterUserFormValidator : AbstractValidator<RegisterUserFormModel>
    {
        public RegisterUserFormValidator()
        {
            RuleFor(p => p.IdCardNumber)
                .NotEmpty()
                .WithMessage("Debe digitar el número de cédula.")
                .MinimumLength(9)
                .WithMessage("La cédula debe de contener al menos 9 caracteres.")
                .MaximumLength(12)
                .WithMessage("La cédula debe de contener máximo 12 caracteres.")
                .Must(isNumeric)
                .WithMessage("Debe digitar un valor numérico.");

            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Debe digitar un nombre.")
                .MaximumLength(20)
                .WithMessage("El nombre debe de tener como máximo 20 caracteres.");

            RuleFor(p => p.FirstLastName)
                .NotEmpty()
                .WithMessage("Debe digitar al menos el primer apellido.")
                .MaximumLength(20)
                .WithMessage("El primer apellido debe de tener como máximo 20 caracteres.");

            RuleFor(p => p.SecondLastName)
                .MaximumLength(20)
                .WithMessage("El segundo apellido debe de tener como máximo 20 caracteres.");

            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage("Debe digitar el correo electrónico del usuario a registrar.")
                .MaximumLength(128)
                .WithMessage("El correo electrónico debe de tener como máximo 128 caracteres.")
                .EmailAddress()
                .WithMessage("Debe digitar un correo electrónico válido.");

            RuleFor(p => p.PrimaryPhoneNumber)
                .NotEmpty()
                .WithMessage("Debe digitar al menos un número teléfonico de contacto.")
                .MaximumLength(8)
                .WithMessage("El número telefónico no puede exceder los 8 números.")
                .MinimumLength(8)
                .WithMessage("El número telefónico debe tener al menos 8 números.")
                .Must(isNumeric)
                .WithMessage("Debe digitar un valor numérico.");


            RuleFor(p => p.SecondaryPhoneNumber)
                .MaximumLength(8)
                .WithMessage("El número telefónico no puede exceder los 8 números.")
                .MinimumLength(8)
                .WithMessage("El número telefónico debe tener al menos 8 números.")
                .Must(isNumeric)
                .WithMessage("Debe digitar un valor numérico.");

            RuleFor(p => p.Profiles)
                .NotEmpty()
                .WithMessage("Debe de seleccionar al menos un perfil.");
        }

        private bool isNumeric(string value)
        {
            if(!String.IsNullOrEmpty(value))
            {
                return int.TryParse(value, out _); 
            }
            return true;
        }
    }
}




