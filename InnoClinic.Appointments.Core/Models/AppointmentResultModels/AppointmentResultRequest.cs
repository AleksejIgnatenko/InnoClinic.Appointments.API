namespace InnoClinic.Appointments.Core.Models.AppointmentResultModels
{
    public record AppointmentResultRequest(
        string Complaints,
        string Conclusion,
        string Recommendations,
        string Diagnosis,
        Guid AppointmentId
        );
}
