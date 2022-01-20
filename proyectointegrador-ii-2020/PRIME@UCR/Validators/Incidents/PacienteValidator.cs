using System;
using System.Text.RegularExpressions;
using FluentValidation;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Validators.Incidents
{
    public class PacienteValidator : AbstractValidator<Paciente>
    {
        public PacienteValidator()
        {
            RuleFor(p => p.Cédula)
                .NotEmpty()
                .WithMessage("Debe digitar una cédula.");

            RuleFor(p => p.Nombre)
                .NotEmpty()
                .WithMessage("Debe digitar un nombre.");

            RuleFor(p => p.Nombre)
                .Must(nombre => !Regex.IsMatch(nombre, "[[{})(*&^%$#@!;,.<>/_0-9~?=+]"))
                .WithMessage("Debe digitar un nombre con caracteres validos"); ;

            RuleFor(p => p.Nombre)
                .MaximumLength(20)
                .WithMessage("Debe digitar un nombre con 20 caracteres o menos.");

            RuleFor(p => p.PrimerApellido)
                .NotEmpty()
                .WithMessage("Debe digitar un primer apellido");

            RuleFor(p => p.PrimerApellido)
                .Must(nombre => !Regex.IsMatch(nombre, "[[{})(*&^%$#@!;,.<>/_0-9~?=+]"))
                .WithMessage("Debe digitar un apellido con caracteres validos");

            RuleFor(p => p.PrimerApellido)
                .MaximumLength(20)
                .WithMessage("Debe digitar un apellido con 20 caracteres o menos.");

            RuleFor(p => p.SegundoApellido)
                 .Must(nombre => nombre == null || !Regex.IsMatch(nombre, "[[{})(*&^%$#@!;,.<>/_0-9~?=+]"))
                 .WithMessage("Debe digitar un apellido con caracteres validos");

            RuleFor(p => p.SegundoApellido)
               .MaximumLength(20)
               .WithMessage("Debe digitar un apellido con 20 caracteres o menos.");

            RuleFor(p => p.FechaNacimiento)
                .Must(f => !f.HasValue || DateTime.Now.AddYears(-120).CompareTo(f.Value) <= 0)
                .WithMessage("No puede exceder los 120 años.");
        }
    }
}