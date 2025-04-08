using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.DataAccess.Repositories;

namespace InnoClinic.Appointments.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task CreateDoctorAsync(DoctorEntity doctorEntity)
    {
        await _doctorRepository.CreateAsync(doctorEntity);
    }

    public async Task UpdateDoctorAsync(DoctorEntity doctorEntity)
    {
        await _doctorRepository.UpdateAsync(doctorEntity);
    }

    public async Task DeleteDoctorAsync(DoctorEntity doctorEntity)
    {
        await _doctorRepository.DeleteAsync(doctorEntity);
    }
}