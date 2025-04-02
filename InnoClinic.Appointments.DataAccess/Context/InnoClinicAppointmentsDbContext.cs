using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;
using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.Core.Models.PatientModels;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Appointments.DataAccess.Context
{
    public class InnoClinicAppointmentsDbContext : DbContext
    {
        public DbSet<AppointmentEntity> Appointments { get; set; }
        public DbSet<AppointmentResultEntity> AppointmentResults { get; set; }
        public DbSet<DoctorEntity> Doctors { get; set; }
        public DbSet<MedicalServiceEntity> MedicalServices { get; set; }
        public DbSet<PatientEntity> Patients { get; set; }

        public InnoClinicAppointmentsDbContext(DbContextOptions<InnoClinicAppointmentsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
