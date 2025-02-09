using InnoClinic.Appointments.Core.Models;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public interface IMedicalServiceRepository : IRepositoryBase<MedicalServiceModel>
    {
        Task<MedicalServiceModel> GetByIdAsync(Guid id);
    }
}