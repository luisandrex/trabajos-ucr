using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using FluentValidation;
using PRIME_UCR.Domain.Models.MedicalRecords;
using FluentValidation.Results;

namespace PRIME_UCR.Validators.Records
{
    public class RecordModelValidator : AbstractValidator<RecordModel>
    {
        public RecordModelValidator() {

            RuleFor(i => i.CedPaciente)
                .NotEmpty()
                .Length(9)
                .WithMessage("Debe ingresar una cedula valida (9 caracteres)")
                .Must(ced => Int32.TryParse(ced, out _));
        }
    }
}
