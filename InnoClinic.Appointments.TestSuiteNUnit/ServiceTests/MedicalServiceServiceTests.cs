using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.DataAccess.Repositories;
using Moq;

namespace InnoClinic.Appointments.TestSuiteNUnit.ServiceTests;

class MedicalServiceServiceTests
{
    private Mock<IMedicalServiceRepository> _medicalServiceRepository;
    private MedicalServiceEntity medicalService;
    private MedicalServiceService _medicalServiceService;

    [SetUp]
    public void SetUp()
    {
        medicalService = new MedicalServiceEntity
        {
            Id = Guid.NewGuid(),
            ServiceName = "Medical Test",
            Price = 100.0m,
            IsActive = true
        };

        _medicalServiceRepository = new Mock<IMedicalServiceRepository>();
        _medicalServiceService = new MedicalServiceService(_medicalServiceRepository.Object);
    }

    [Test]
    public async Task CreateMedicalServiceAsync_ValidMedicalService_CallsCreateAsync()
    {
        // Act
        await _medicalServiceService.CreateMedicalServiceAsync(medicalService);

        // Assert
        _medicalServiceRepository.Verify(repo => repo.CreateAsync(medicalService), Times.Once);
    }

    [Test]
    public async Task UpdateMedicalServiceAsync_ValidMedicalService_CallsUpdateAsync()
    {
        // Act
        await _medicalServiceService.UpdateMedicalServiceAsync(medicalService);

        // Assert
        _medicalServiceRepository.Verify(repo => repo.UpdateAsync(medicalService), Times.Once);
    }

    [Test]
    public async Task DeleteMedicalServiceAsync_ValidMedicalService_CallsDeleteAsync()
    {
        // Act
        await _medicalServiceService.DeleteMedicalServiceAsync(medicalService);

        // Assert
        _medicalServiceRepository.Verify(repo => repo.DeleteAsync(medicalService), Times.Once);
    }
}
