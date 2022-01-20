using System;
using FluentValidation;
using PRIME_UCR.Application.Dtos.Incidents;

namespace PRIME_UCR.Validators.Incidents
{
    public class FeedBackModelValidator : AbstractValidator<IncidentFeedbackModel>
    {
        public FeedBackModelValidator()
        {
            RuleFor(i => i.FeedBack)
                .NotEmpty()                
                .WithMessage("La retroalimentación no debe estar vacía.");
            RuleFor(i => i.FeedBack)
                .MaximumLength(199)
                .WithMessage("La retroalimentación debe ser como máximo de 200 caracteres.");
        }
    }
}
