using InnoClinic.Appointments.Core.Models;

namespace InnoClinic.Appointments.Application.Services
{
    public interface IValidationService
    {
        Dictionary<string, string> Validation(AppointmentModel appointmentModel);
    }
}