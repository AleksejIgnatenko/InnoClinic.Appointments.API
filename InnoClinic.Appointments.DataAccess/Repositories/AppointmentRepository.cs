using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public class AppointmentRepository : RepositoryBase<AppointmentEntity>, IAppointmentRepository
    {
        public AppointmentRepository(InnoClinicAppointmentsDbContext context) : base(context) { }

        public async Task<IEnumerable<AppointmentEntity>> GetAllAsync()
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.MedicalService)
                .ToListAsync();
        }

        public async Task<AppointmentEntity> GetByIdAsync(Guid id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.MedicalService)
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new DataRepositoryException("Appointment not found", 404);
        }

        public async Task<IEnumerable<AppointmentEntity>> GetAllByDoctorAccountIdAsync(Guid accountId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalService)
                .Where(a => a.Doctor.AccountId.Equals(accountId))
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentEntity>> GetAllByPatientIdAsync(Guid patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalService)
                .Where(a => a.Patient.Id.Equals(patientId))
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentEntity>> GetByDateAsync(string date)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.MedicalService)
                .Where(a => a.Date.Equals(date))
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentEntity>> GetByAccountIdAndDateAsync(Guid accountId, string date)
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
