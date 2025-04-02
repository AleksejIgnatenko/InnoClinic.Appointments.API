using InnoClinic.Appointments.Core.Models.AppointmentResultModels;

namespace InnoClinic.Appointments.Application.Services
{
    public interface IAppointmentResultService
    {
        Task CreateAppointmentResultAsync(string complaints, string conclusion, string recomendations, string diagnisis, Guid appointmentId);
        Task DeleteAppointmentResultAsync(Guid id);
        Task<IEnumerable<AppointmentResultEntity>> GetAllAppointmentResultsAsync();
        Task<IEnumerable<AppointmentResultEntity>> GetAllAppointmentResultsByAppointmentIdAsync(Guid appointmentId);
        Task UpdateAppointmentResultAsync(Guid id, string complaints, string conclusion, string recomendations, string diagnisis, Guid appointmentId);
    }
}