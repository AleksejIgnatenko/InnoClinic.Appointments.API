using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.DataAccess.Repositories;

namespace InnoClinic.Appointments.Application.Services;

public class MedicalServiceService : IMedicalServiceService
{
    private readonly IMedicalServiceRepository _medicalServiceRepository;

    public MedicalServiceService(IMedicalServiceRepository medicalServiceRepository)
    {
        _medicalServiceRepository = medicalServiceRepository;
    }

    public async Task CreateMedicalServiceAsync(MedicalServiceEntity medicalServiceEntity)
    {
        await _medicalServiceRepository.CreateAsync(medicalServiceEntity);
    }

    public async Task UpdateMedicalServiceAsync(MedicalServiceEntity medicalServiceEntity)
    {
        await _medicalServiceRepository.UpdateAsync(medicalServiceEntity);
    }

    public async Task DeleteMedicalServiceAsync(MedicalServiceEntity medicalServiceEntity)
    {
        await _medicalServiceRepository.DeleteAsync(medicalServiceEntity);
    }
}