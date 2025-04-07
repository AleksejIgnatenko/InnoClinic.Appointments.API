namespace InnoClinic.Appointments.Core.Models.AppointmentModels;

public record SendNotificationAboutAppointmentRequest(
    Guid AccountId,
    string PatientFullName,
    string Date,
    string Time,
    string MedicalServiceName,
    string DoctorFullName
);