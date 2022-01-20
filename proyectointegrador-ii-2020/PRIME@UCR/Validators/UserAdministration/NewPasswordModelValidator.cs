using FluentValidation;
using PRIME_UCR.Application.DTOs.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Validators.UserAdministration
{
    public class NewPasswordModelValidator : AbstractValidator<NewPasswordModel>
    {
        public NewPasswordModelValidator()
        {
            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage("Debe de indicar una contraseña")
                .Must(HasUpperCase)
                .WithMessage("La contraseña debe de tener mayúsculas")
                .Must(HasDigits)
                .WithMessage("La contraseña debe de tener dígitos")
                .Must(HasSymbols)
                .WithMessage("La contraseña debe de tener simbolos")
                .Must(HasLowerCases)
                .WithMessage("La contraseña debe de tener minúsculas")
                .Must(HasMinumumLength)
                .WithMessage("La contraseña debe de tener al menos 8 caracteres")
                .Must(HasMaximumLength)
                .WithMessage("La contraseña debe de tener como máximo 128 caracteres");

            RuleFor(p => p.ConfirmedPassword)
                .NotEmpty()
                .WithMessage("Debe de indicar una contraseña")
                .Must(HasUpperCase)
                .WithMessage("La contraseña debe de tener mayúsculas")
                .Must(HasDigits)
                .WithMessage("La contraseña debe de tener dígitos")
                .Must(HasSymbols)
                .WithMessage("La contraseña debe de tener simbolos")
                .Must(HasLowerCases)
                .WithMessage("La contraseña debe de tener minúsculas")
                .Must(HasMinumumLength)
                .WithMessage("La contraseña debe de tener al menos 8 caracteres")
                .Must(HasMaximumLength)
                .WithMessage("La contraseña debe de tener como máximo 128 caracteres")
                .Must((model, currentPassword) => model.Password == currentPassword)
                .WithMessage("Las contraseñas deben ser iguales");
        }

        public bool HasUpperCase(string password)
        {
            return password.ToArray().Any(Char.IsUpper);
        }

        public bool HasDigits(string password)
        {
            return password.ToArray().Any(Char.IsDigit);
        }

        public bool HasSymbols(string password)
        {
            return password.ToArray().Any(Char.IsSymbol) || password.ToArray().Any(Char.IsPunctuation);
        }

        public bool HasLowerCases(string password)
        {
            return password.ToArray().Any(Char.IsLower);
        }

        public bool HasMinumumLength(string password)
        {
            return password.Length >= 8;
        }

        public bool HasMaximumLength(string password)
        {
            return password.Length <= 128;
        }
    }
}
