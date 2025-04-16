using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Appointments.DataAccess.Repositories;

namespace InnoClinic.Appointments.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task CreatePatientAsync(PatientEntity patientEntity)
    {
        await _patientRepository.CreateAsync(patientEntity);
    }

    public async Task UpdatePatientAsync(PatientEntity patientEntity)
    {
        await _patientRepository.UpdateAsync(patientEntity);
    }

    public async Task DeletePatientAsync(PatientEntity patientEntity)
    {
        await _patientRepository.DeleteAsync(patientEntity);
    }
}