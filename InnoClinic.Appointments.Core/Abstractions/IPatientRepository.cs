using InnoClinic.Appointments.Core.Models.PatientModels;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public interface IPatientRepository : IRepositoryBase<PatientEntity>
    {
        Task<PatientEntity> GetByIdAsync(Guid id);
        Task<PatientEntity> GetByAccountIdAsync(Guid accountId);
    }
}