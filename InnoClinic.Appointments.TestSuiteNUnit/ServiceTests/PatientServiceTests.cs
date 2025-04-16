using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Appointments.DataAccess.Repositories;
using Moq;

namespace InnoClinic.Appointments.TestSuiteNUnit.ServiceTests;

class PatientServiceTests
{
    private Mock<IPatientRepository> _patientRepository;
    private PatientEntity patient;
    private PatientService _patientService;

    [SetUp]
    public void SetUp()
    {
        patient = new PatientEntity
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            MiddleName = "Smith",
            AccountId = Guid.NewGuid()
        };

        _patientRepository = new Mock<IPatientRepository>();
        _patientService = new PatientService(_patientRepository.Object);

    }

    [Test]
    public async Task CreatePatientAsync_ValidPatient_CallsCreateAsync()
    {
        // Act
        await _patientService.CreatePatientAsync(patient);

        // Assert
        _patientRepository.Verify(repo => repo.CreateAsync(patient), Times.Once);
    }

    [Test]
    public async Task UpdatePatientAsync_ValidPatient_CallsUpdateAsync()
    {
        // Act
        await _patientService.UpdatePatientAsync(patient);

        // Assert
        _patientRepository.Verify(repo => repo.UpdateAsync(patient), Times.Once);
    }

    [Test]
    public async Task DeletePatientAsync_ValidPatient_CallsDeleteAsync()
    {
        // Act
        await _patientService.DeletePatientAsync(patient);

        // Assert
        _patientRepository.Verify(repo => repo.DeleteAsync(patient), Times.Once);
    }
}
