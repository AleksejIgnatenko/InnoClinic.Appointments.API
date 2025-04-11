using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.DataAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public class MedicalServiceRepository : RepositoryBase<MedicalServiceEntity>, IMedicalServiceRepository
    {
        public MedicalServiceRepository(InnoClinicAppointmentsDbContext context) : base(context) { }

        public async Task<MedicalServiceEntity> GetByIdAsync(Guid id)
        {
            return await _context.MedicalServices
                .FirstOrDefaultAsync(m => m.Id.Equals(id))
                ?? throw new DataRepositoryException($"Service with Id '{id}' not found.", StatusCodes.Status404NotFound); ;
        }
    }
}
