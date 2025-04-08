using InnoClinic.Appointments.Core.Models.MedicalServiceModels;

namespace InnoClinic.Appointments.Application.Services;

public interface IMedicalServiceService
{
    Task CreateMedicalServiceAsync(MedicalServiceEntity medicalServiceEntity);
    Task UpdateMedicalServiceAsync(MedicalServiceEntity medicalServiceEntity);
    Task DeleteMedicalServiceAsync(MedicalServiceEntity medicalServiceEntity);
}