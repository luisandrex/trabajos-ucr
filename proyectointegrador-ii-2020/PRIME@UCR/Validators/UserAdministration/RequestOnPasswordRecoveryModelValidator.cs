using FluentValidation;
using PRIME_UCR.Application.DTOs.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Validators.UserAdministration
{
    public class RequestOnPasswordRecoveryModelValidator : AbstractValidator<RequestOnPasswordRecoveryModel>
    {
        public RequestOnPasswordRecoveryModelValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Debe digitar su correo electrónico.")
                .EmailAddress()
                .WithMessage("Debe digitar un correo electrónico válido.");
        }
    }
}
