using InnoClinic.Appointments.Core.Models.AppointmentResultModels;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public interface IAppointmentResultRepository : IRepositoryBase<AppointmentResultEntity>
    {
        Task<IEnumerable<AppointmentResultEntity>> GetAllAsync();
        Task<AppointmentResultEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<AppointmentResultEntity>> GetAllByAppointmentIdAsync(Guid appointmentId);
    }
}