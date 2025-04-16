using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.DataAccess.Repositories;
using Moq;

namespace InnoClinic.Appointments.TestSuiteNUnit.ServiceTests;

class DoctorServiceTests
{
    private Mock<IDoctorRepository> _doctorRepository;
    private DoctorEntity doctor;
    private DoctorService _doctorService;

    [SetUp]
    public void SetUp()
    {
        doctor = new DoctorEntity
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName",
            MiddleName = "MiddleName",
            CabinetNumber = 1,
            Status = "At work",
        };

        _doctorRepository = new Mock<IDoctorRepository>();

        _doctorService = new DoctorService(
            _doctorRepository.Object);
    }

    [Test]
    public async Task CreateDoctorAsync_ValidDoctor_CallsCreateAsync()
    {
        // Act
        await _doctorService.CreateDoctorAsync(doctor);

        // Assert
        _doctorRepository.Verify(repo => repo.CreateAsync(doctor), Times.Once);
    }

    [Test]
    public async Task UpdateDoctorAsync_ValidDoctor_CallsUpdateAsync()
    {
        // Act
        await _doctorService.UpdateDoctorAsync(doctor);

        // Assert
        _doctorRepository.Verify(repo => repo.UpdateAsync(doctor), Times.Once);
    }

    [Test]
    public async Task DeleteDoctorAsync_ValidDoctor_CallsDeleteAsync()
    {
        // Act
        await _doctorService.DeleteDoctorAsync(doctor);

        // Assert
        _doctorRepository.Verify(repo => repo.DeleteAsync(doctor), Times.Once);
    }
}
