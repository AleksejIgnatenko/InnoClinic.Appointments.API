using FluentValidation;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;

namespace InnoClinic.Appointments.Application.Validators
{
    public class AppointmentResultValidator : AbstractValidator<AppointmentResultEntity>
    {
        public AppointmentResultValidator()
        {
            RuleFor(x => x.Complaints)
                .NotEmpty().WithMessage("The complaints field is required.");

            RuleFor(x => x.Conclusion)
                .NotEmpty().WithMessage("The conclusion field is required.");

            RuleFor(x => x.Recomendations)
                .NotEmpty().WithMessage("The recomendations field is required.");

            RuleFor(x => x.Diagnisis)
                .NotEmpty().WithMessage("The diagnisis field is required.");
        }
    }
}
