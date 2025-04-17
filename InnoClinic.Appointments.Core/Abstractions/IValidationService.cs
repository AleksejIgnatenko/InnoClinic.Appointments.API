using FluentValidation.Results;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;

namespace InnoClinic.Appointments.Application.Services
{
    public interface IValidationService
    {
        List<ValidationFailure> Validation(AppointmentEntity appointmentModel);
        List<ValidationFailure> Validation(AppointmentResultEntity appointmentResultModel);
    }
}