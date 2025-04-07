using InnoClinic.Appointments.Core.Models.DoctorModels;

namespace InnoClinic.Appointments.Application.Services;

public interface IDoctorService
{
    Task CreateDoctorAsync(DoctorEntity doctorEntity);
    Task UpdateDoctorAsync(DoctorEntity doctorEntity);
    Task DeleteDoctorAsync(DoctorEntity doctorEntity);
}