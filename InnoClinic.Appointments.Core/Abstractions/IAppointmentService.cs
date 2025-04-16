using InnoClinic.Appointments.Core.Models.AppointmentModels;

namespace InnoClinic.Appointments.Application.Services
{
    public interface IAppointmentService
    {
        Task CreateAppointmentAsync(Guid patientId, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved);
        Task CreateAppointmentAsync(string token, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved);
        Task DeleteAppointmentAsync(Guid id);
        Task<IEnumerable<AppointmentEntity>> GetAllAppointmentsAsync();
        Task UpdateAppointmentAsync(Guid id, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved);
        Task<IEnumerable<AppointmentEntity>> GetDoctorAppointmentsByAccessTokenAsync(string token);
        Task<IEnumerable<AppointmentEntity>> GetAppointmentsByDateAsync(string date);
        Task<IEnumerable<AppointmentEntity>> GetDoctorAppointmentsByAccessTokenAndDateAsync(string token, string date);
        Task<List<string>> GetAllAvailableTimeSlotsAsync(string date, int timeSlotSize, Guid doctorId);
        Task<IEnumerable<AppointmentEntity>> GetAllAppointmentsByPatientIdAsync(Guid patientId);
        Task<bool> IsAppointmentResultsExistenceAsync(Guid id);
    }
}