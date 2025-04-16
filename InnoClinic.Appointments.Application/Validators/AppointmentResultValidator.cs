using FluentValidation;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;

namespace InnoClinic.Appointments.Application.Validators;

internal class AppointmentResultValidator : AbstractValidator<AppointmentResultEntity>
{
    public AppointmentResultValidator()
    {
        RuleFor(x => x.Complaints)
            .NotEmpty().WithMessage("The complaints field is required.");

        RuleFor(x => x.Conclusion)
            .NotEmpty().WithMessage("The conclusion field is required.");

        RuleFor(x => x.Recommendations)
            .NotEmpty().WithMessage("The recommendations field is required.");

        RuleFor(x => x.Diagnosis)
            .NotEmpty().WithMessage("The diagnosis field is required.");
    }
}
