using InnoClinic.Appointments.Core.Models.AppointmentModels;

namespace InnoClinic.Appointments.Application.Services
{
    public interface IAppointmentService
    {
        Task CreateAppointmentAsync(string token, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved);
        Task DeleteAppointmentAsync(Guid id);
        Task<IEnumerable<AppointmentEntity>> GetAllAppointmentsAsync();
        Task UpdateAppointmentAsync(Guid id, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved);
        Task<IEnumerable<AppointmentEntity>> GetAppointmentsByDoctorAsync(string token);
        Task<IEnumerable<AppointmentEntity>> GetAppointmentsByDateAsync(string date);
        Task<IEnumerable<AppointmentEntity>> GetAppointmentsByDoctorAndDateAsync(string token, string date);
        Task<List<string>> GetAllAvailableTimeSlotsAsync(string date, int timeSlotSize);
        Task<IEnumerable<AppointmentEntity>> GetAllAppointmentsByPatientIdAsync(Guid patientId);
        Task<bool> IsAppointmentResultsExistenceAsync(Guid id);
    }
}