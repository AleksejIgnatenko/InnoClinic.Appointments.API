using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;

namespace InnoClinic.Appointments.Application.Services
{
    public interface IValidationService
    {
        Dictionary<string, string> Validation(AppointmentEntity appointmentModel);
        Dictionary<string, string> Validation(AppointmentResultEntity appointmentResultModel);
    }
}