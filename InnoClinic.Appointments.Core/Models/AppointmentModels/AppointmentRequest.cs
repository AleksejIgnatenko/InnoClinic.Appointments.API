namespace InnoClinic.Appointments.Core.Models.AppointmentModels
{
    public record AppointmentRequest(
        Guid DoctorId,
        Guid MedicalServiceId,
        string Date,
        string Time,
        bool IsApproved
        );
}
