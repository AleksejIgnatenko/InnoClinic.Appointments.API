using InnoClinic.Appointments.Core.Dto;
using InnoClinic.Appointments.Core.Models;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public interface IDoctorRepository : IRepositoryBase<DoctorModel>
    {
        Task<DoctorModel> GetByIdAsync(Guid id);
    }
}