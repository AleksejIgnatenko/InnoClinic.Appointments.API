using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models;
using InnoClinic.Appointments.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public class AppointmentRepository : RepositoryBase<AppointmentModel>, IAppointmentRepository
    {
        public AppointmentRepository(InnoClinicAppointmentsDbContext context) : base(context) { }

        public async Task<IEnumerable<AppointmentModel>> GetAllAsync()
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.MedicalService)
                .ToListAsync();
        }

        public async Task<AppointmentModel> GetByIdAsync(Guid id)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new DataRepositoryException("Appointment not found", 404);
        }

        public async Task<IEnumerable<AppointmentModel>> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalService)
                .Where(a => a.Doctor.AccountId.Equals(accountId))
                .ToListAsync();
        }
        public async Task<IEnumerable<AppointmentModel>> GetByDateAsync(string date)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalService)
                .Where(a => a.Date.Equals(date))
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentModel>> GetByAccountIdAndDateAsync(Guid accountId, string date)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalService)
                .Where(a => (a.Doctor.AccountId.Equals(accountId)) && (a.Date.Equals(date)))
                .ToListAsync();
        }

    }
}
