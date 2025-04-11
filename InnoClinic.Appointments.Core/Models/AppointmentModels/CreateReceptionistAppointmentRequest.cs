namespace InnoClinic.Appointments.Core.Models.AppointmentModels
{
    public record CreateReceptionistAppointmentRequest(
        Guid PatientId,
        Guid DoctorId,
        Guid MedicalServiceId,
        string Date,
        string Time,
        bool IsApproved
        );
}
