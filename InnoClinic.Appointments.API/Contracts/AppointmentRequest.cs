namespace InnoClinic.Appointments.API.Contracts
{
    public record AppointmentRequest(
        Guid DoctorId,
        Guid MedicalServiceId,
        Guid PatientId,
        string Date,
        string Time,
        bool IsApproved
        );
}
