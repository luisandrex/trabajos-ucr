using FluentValidation;
using FluentValidation.Results;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIME_UCR.Validators.Records
{
    public class RecordValidator : AbstractValidator<Expediente>
    {
        public RecordValidator() {
            RuleFor(i => i.CedulaMedicoDuenno)
                .NotEmpty()
                .Length(8)
                .WithMessage("Ingrese la cedula de un funcionario (9 caracteres)")
                .Must(ced => Int32.TryParse(ced, out _));
        }
    }
}
