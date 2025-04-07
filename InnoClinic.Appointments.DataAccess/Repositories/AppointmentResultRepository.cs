using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;
using InnoClinic.Appointments.DataAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public class AppointmentResultRepository : RepositoryBase<AppointmentResultEntity>, IAppointmentResultRepository
    {
        public AppointmentResultRepository(InnoClinicAppointmentsDbContext context) : base(context) { }

        public async Task<IEnumerable<AppointmentResultEntity>> GetAllAsync()
        {
            return await _context.AppointmentResults
                .AsNoTracking()
                .Include(ar => ar.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Include(ar => ar.Appointment)
                    .ThenInclude(a => a.MedicalService)
                .Include(ar => ar.Appointment)
                    .ThenInclude(a => a.Patient)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentResultEntity>> GetAllByAppointmentIdAsync(Guid appointmentId)
        {
            return await _context.AppointmentResults
                .AsNoTracking()
                .Include(ar => ar.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Include(ar => ar.Appointment)
                    .ThenInclude(a => a.MedicalService)
                .Include(ar => ar.Appointment)
                    .ThenInclude(a => a.Patient)
                .Where(ar => ar.Appointment.Id.Equals(appointmentId))
                .ToListAsync();
        }

        public async Task<AppointmentResultEntity> GetByIdAsync(Guid id)
        {
            return await _context.AppointmentResults
                .AsNoTracking()
                .Include(ar => ar.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Include(ar => ar.Appointment)
                    .ThenInclude(a => a.MedicalService)
                .Include(ar => ar.Appointment)
                    .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id.Equals(id))
                ?? throw new DataRepositoryException($"Appointment Results with Id '{id}' not found.", StatusCodes.Status404NotFound);
        }

        public async Task<bool> IsAppointmentResultsExistenceAsync(Guid appointmentId)
        {
            return await _context.AppointmentResults
                .AnyAsync(a => a.Appointment.Id.Equals(appointmentId));
        }
    }
}
