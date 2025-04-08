using System.Threading.Channels;
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
        Console.WriteLine("111111111111111111111");
        await _patientRepository.CreateAsync(patientEntity);
    }

    public async Task UpdatePatientAsync(PatientEntity patientEntity)
    {
        Console.WriteLine("22222222222222222222222");
        await _patientRepository.UpdateAsync(patientEntity);
    }

    public async Task DeletePatientAsync(PatientEntity patientEntity)
    {
        Console.WriteLine("33333333333333333333");
        await _patientRepository.DeleteAsync(patientEntity);
    }
}