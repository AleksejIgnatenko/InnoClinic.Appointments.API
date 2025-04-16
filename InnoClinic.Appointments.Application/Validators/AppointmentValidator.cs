using FluentValidation;
using InnoClinic.Appointments.Core.Models.AppointmentModels;

namespace InnoClinic.Appointments.Application.Validators;

internal class AppointmentValidator : AbstractValidator<AppointmentEntity>
{
    public AppointmentValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Date must be in the format yyyy-MM-dd.");
    }
}
