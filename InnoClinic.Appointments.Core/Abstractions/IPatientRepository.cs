using InnoClinic.Appointments.Core.Models;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public interface IPatientRepository : IRepositoryBase<PatientModel>
    {
        Task<PatientModel> GetByIdAsync(Guid id);
        Task<PatientModel> GetByAccountIdAsync(Guid accountId);
    }
}