using InnoClinic.Appointments.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Appointments.DataAccess.Context
{
    public class InnoClinicAppointmentsDbContext : DbContext
    {
        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<DoctorModel> Doctors { get; set; }
        public DbSet<MedicalServiceModel> MedicalServices { get; set; }
        public DbSet<PatientModel> Patients { get; set; }

        public InnoClinicAppointmentsDbContext(DbContextOptions<InnoClinicAppointmentsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
