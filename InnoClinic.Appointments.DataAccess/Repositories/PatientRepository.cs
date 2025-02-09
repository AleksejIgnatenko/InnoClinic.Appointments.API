using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models;
using InnoClinic.Appointments.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public class PatientRepository : RepositoryBase<PatientModel>, IPatientRepository
    {
        public PatientRepository(InnoClinicAppointmentsDbContext context) : base(context) { }

        public async Task<PatientModel> GetByIdAsync(Guid id)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new DataRepositoryException("Patient not found", 404);
        }

        public async Task<PatientModel> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.AccountId == accountId)
                ?? throw new DataRepositoryException("Patient not found", 404);
        }

        public override async Task UpdateAsync(PatientModel entity)
        {
            await _context.Patients
                .Where(p => p.Id.Equals(entity.Id))
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.FirstName, entity.FirstName)
                    .SetProperty(p => p.LastName, entity.LastName)
                    .SetProperty(p => p.MiddleName, entity.MiddleName)
                );
        }

        public override async Task DeleteAsync(PatientModel entity)
        {
            await _context.Patients
                .Where(p => p.Id.Equals(entity.Id))
                .ExecuteDeleteAsync();
        }
    }
}
