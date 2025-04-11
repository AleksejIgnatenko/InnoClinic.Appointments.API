using InnoClinic.Appointments.Core.Models.AppointmentModels;

namespace InnoClinic.Appointments.DataAccess.Repositories;

public interface IAppointmentRepository : IRepositoryBase<AppointmentEntity>
{
    Task<IEnumerable<AppointmentEntity>> GetAllAsync();
    Task<AppointmentEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<AppointmentEntity>> GetAllByDoctorAccountIdAsync(Guid accountId);
    Task<IEnumerable<AppointmentEntity>> GetByDateAsync(string date);
    Task<IEnumerable<AppointmentEntity>> GetByAccountIdAndDateAsync(Guid accountId, string date);
    Task<IEnumerable<AppointmentEntity>> GetAllByPatientIdAsync(Guid patientId);
    Task<IEnumerable<AppointmentEntity>> GetByDateAndDoctorIdAsync(string date, Guid doctorId);
}