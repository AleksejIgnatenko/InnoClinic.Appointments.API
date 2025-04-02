namespace InnoClinic.Appointments.Core.Models.AppointmentResultModels
{
    public record AppointmentResultRequest(
        string Complaints,
        string Conclusion,
        string Recomendations,
        string Diagnisis,
        Guid AppointmentId
        );
}
