using InnoClinic.Appointments.Core.Models;

namespace InnoClinic.Appointments.Application.Services
{
    public interface IAppointmentService
    {
        Task CreateAppointmentAsync(string token, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved);
        Task DeleteAppointmentAsync(Guid id);
        Task<IEnumerable<AppointmentModel>> GetAllAppointmentsAsync();
        Task UpdateAppointmentAsync(Guid id, string token, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved);
        Task<IEnumerable<AppointmentModel>> GetAppointmentsByDoctorAsync(string token);
        Task<IEnumerable<AppointmentModel>> GetAppointmentsByDoctorAndDateAsync(string token, string date);
    }
}