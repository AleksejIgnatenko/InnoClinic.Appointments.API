using InnoClinic.Appointments.Core.Models.MedicalServiceModels;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public interface IMedicalServiceRepository : IRepositoryBase<MedicalServiceEntity>
    {
        Task<MedicalServiceEntity> GetByIdAsync(Guid id);
    }
}