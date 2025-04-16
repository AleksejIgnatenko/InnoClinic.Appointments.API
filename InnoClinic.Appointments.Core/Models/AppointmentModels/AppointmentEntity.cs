using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.Core.Models.PatientModels;

namespace InnoClinic.Appointments.Core.Models.AppointmentModels;

public class AppointmentEntity
{
    public Guid Id { get; set; }
    public PatientEntity Patient { get; set; } = new PatientEntity();
    public DoctorEntity Doctor { get; set; } = new DoctorEntity();
    public MedicalServiceEntity MedicalService { get; set; } = new MedicalServiceEntity();
    public string Date {  get; set; } = string.Empty;
    public string Time {  get; set; } = string.Empty;
    public bool IsApproved { get; set; }
}