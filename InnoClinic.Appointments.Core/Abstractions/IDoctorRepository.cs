using InnoClinic.Appointments.Core.Models.DoctorModels;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public interface IDoctorRepository : IRepositoryBase<DoctorEntity>
    {
        Task<DoctorEntity> GetByIdAsync(Guid id);
    }
}