using FluentValidation;
using PRIME_UCR.Application.DTOs.UserAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Validators.UserAdministration
{
    public class UserValidationModelValidator : AbstractValidator<UserValidationInfoModel>
    {
        public UserValidationModelValidator()
        {
            RuleFor(p => p.PasswordModel)
                .SetValidator(new NewPasswordModelValidator());
        }
    }
}