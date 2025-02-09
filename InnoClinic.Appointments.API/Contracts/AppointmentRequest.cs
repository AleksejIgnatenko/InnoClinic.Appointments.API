namespace InnoClinic.Appointments.API.Contracts
{
    public record AppointmentRequest(
        Guid DoctorId,
        Guid MedicalServiceId,
        string Date,
        string Time,
        bool IsApproved
        );
}
