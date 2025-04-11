using InnoClinic.Appointments.Core.Models.AppointmentModels;

namespace InnoClinic.Appointments.Core.Models.AppointmentResultModels;

public class AppointmentResultEntity
{
    public Guid Id { get; set; }
    public string Complaints { get; set; } = string.Empty; 
    public string Conclusion { get; set; } = string.Empty; 
    public string Recommendations { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public AppointmentEntity Appointment { get; set; } = new AppointmentEntity();
}
