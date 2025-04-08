using InnoClinic.Appointments.Core.Models.PatientModels;

namespace InnoClinic.Appointments.Application.Services;

public interface IPatientService
{
    Task CreatePatientAsync(PatientEntity patientEntity);
    Task UpdatePatientAsync(PatientEntity patientEntity);
    Task DeletePatientAsync(PatientEntity patientEntity);
}