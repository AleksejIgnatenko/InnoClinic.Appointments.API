using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;
using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Appointments.DataAccess.Repositories;
using Moq;

namespace InnoClinic.Appointments.TestSuiteNUnit.ServiceTests;

public class AppointmentResultServiceTests
{
    private Mock<IAppointmentResultRepository> _appointmentResultRepositoryMock;
    private Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private Mock<IValidationService> _validationServiceMock;

    private readonly AppointmentResultService _appointmentResultService;
    private AppointmentEntity appointment;
    private AppointmentResultEntity appointmentResult;

    public AppointmentResultServiceTests()
    {
        appointment = new AppointmentEntity
        {
            Id = Guid.NewGuid(),
            Patient = new PatientEntity(),
            Doctor = new DoctorEntity(),
            MedicalService = new MedicalServiceEntity(),
            Date = "2025-01-01",
            Time = "08:00 - 08:10",
            IsApproved = false,
        };

        appointmentResult = new AppointmentResultEntity
        {
            Id = Guid.NewGuid(),
            Complaints = "Complaints",
            Conclusion = "Conclusion",
            Recommendations = "Recommendations",
            Diagnosis = "Diagnosis",
            Appointment = new AppointmentEntity(),
        };

        _appointmentResultRepositoryMock = new Mock<IAppointmentResultRepository>();
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _validationServiceMock = new Mock<IValidationService>();

        _appointmentResultService = new AppointmentResultService(
                _appointmentResultRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _validationServiceMock.Object
        );
    }

    [Test]
    public async Task CreateAppointmentResultAsync_ValidAppointment_ResultCreated()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(appointment.Id)).ReturnsAsync(appointment);

        _validationServiceMock.Setup(service => service.Validation(appointment)).Returns(new Dictionary<string, string>());

        _appointmentResultRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<AppointmentResultEntity>()))
            .Returns(Task.FromResult(appointmentResult));

        // Act
        await _appointmentResultService.CreateAppointmentResultAsync("Complaints", "Conclusion", "Recommendations", "Diagnosis", appointment.Id);

        // Assert
        _appointmentRepositoryMock.Verify(repo => repo.GetByIdAsync(appointment.Id), Times.Once);
        _appointmentResultRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<AppointmentResultEntity>()), Times.Once);
    }

    [Test]
    public void CreateAppointmentResultAsync_InvalidAppointment_ThrowsValidationException()
    {
        // Arrange
        var validationErrors = new Dictionary<string, string>
        {
            { "Complaints", "The complaints field is required." },
            { "Conclusion", "The conclusion field is required." },
            { "Recommendations", "The recommendations field is required." },
            { "Diagnosis", "The diagnosis field is required." },
        };

        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(appointment.Id)).ReturnsAsync(appointment);

        _validationServiceMock.Setup(service => service.Validation(appointment)).Returns(validationErrors);

        _appointmentResultRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<AppointmentResultEntity>()))
            .Returns(Task.FromResult(appointmentResult));

        // Act and Assert
        ValidationException exception = Assert.ThrowsAsync<ValidationException>(async () =>
            await _appointmentResultService.CreateAppointmentResultAsync("", "", "", "", appointment.Id));

        Assert.NotNull(exception);
        Assert.AreEqual(validationErrors, exception.Errors);

        _appointmentRepositoryMock.Verify(repo => repo.GetByIdAsync(appointment.Id), Times.Once);
        _appointmentResultRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<AppointmentResultEntity>()), Times.Never);
    }

    [Test]
    public async Task GetAllAppointmentResultsAsync_ShouldReturnAllResults()
    {
        // Arrange
        var appointmentResults = new List<AppointmentResultEntity>
        {
            new AppointmentResultEntity { Id = Guid.NewGuid(), Complaints = "Complaints 1", Conclusion = "Conclusion 1" },
            new AppointmentResultEntity { Id = Guid.NewGuid(), Complaints = "Complaints 2", Conclusion = "Conclusion 2" }
        };

        _appointmentResultRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(appointmentResults);

        // Act
        var results = await _appointmentResultService.GetAllAppointmentResultsAsync();

        // Assert
        Assert.NotNull(results);
        Assert.AreEqual(appointmentResults.Count, results.Count());

        _appointmentResultRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetAllAppointmentResultsByAppointmentIdAsync_ShouldReturnResultsByAppointmentId()
    {
        // Arrange
        Guid appointmentId = Guid.NewGuid();
        var appointmentResults = new List<AppointmentResultEntity>
        {
            new AppointmentResultEntity { Id = Guid.NewGuid(), Appointment = new AppointmentEntity { Id =  appointmentId }, Complaints = "Complaints 1", Conclusion = "Conclusion 1" },
            new AppointmentResultEntity { Id = Guid.NewGuid(), Appointment = new AppointmentEntity { Id = appointmentId }, Complaints = "Complaints 2", Conclusion = "Conclusion 2" }
        };

        _appointmentResultRepositoryMock.Setup(repo => repo.GetAllByAppointmentIdAsync(appointmentId)).ReturnsAsync(appointmentResults);

        // Act
        var results = await _appointmentResultService.GetAllAppointmentResultsByAppointmentIdAsync(appointmentId);

        // Assert
        Assert.NotNull(results);
        Assert.AreEqual(appointmentResults.Count, results.Count());
    }

    [Test]
    public void UpdateAppointmentResultAsync_ShouldUpdateResult()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(appointment.Id)).ReturnsAsync(appointment);
        _validationServiceMock.Setup(service => service.Validation(appointment)).Returns(new Dictionary<string, string>());
        _appointmentResultRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<AppointmentResultEntity>())).Returns(Task.CompletedTask);

        // Act and Assert
        Assert.DoesNotThrowAsync(async () => await _appointmentResultService.UpdateAppointmentResultAsync(Guid.NewGuid(), "Complaints", "Conclusion", "Recommendations", "Diagnosis", Guid.NewGuid()));

        _appointmentRepositoryMock.Verify(repo => repo.GetByIdAsync(appointment.Id), Times.Once);
        _appointmentResultRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<AppointmentResultEntity>()), Times.Once);
    }

    [Test]
    public void UpdateAppointmentResultAsync_InvalidAppointment_ThrowsValidationException()
    {
        // Arrange
        var validationErrors = new Dictionary<string, string>
        {
            { "Complaints", "The complaints field is required." },
            { "Conclusion", "The conclusion field is required." },
            { "Recommendations", "The recommendations field is required." },
            { "Diagnosis", "The diagnosis field is required." },
        };

        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(appointment.Id)).ReturnsAsync(appointment);
        _validationServiceMock.Setup(service => service.Validation(appointment)).Returns(validationErrors);
        _appointmentResultRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<AppointmentResultEntity>())).Returns(Task.CompletedTask);

        // Act and Assert
        ValidationException exception = Assert.ThrowsAsync<ValidationException>(async () =>
            await _appointmentResultService.UpdateAppointmentResultAsync(Guid.NewGuid(), "", "", "", "", appointment.Id));

        Assert.NotNull(exception);
        Assert.AreEqual(validationErrors, exception.Errors);

        _appointmentRepositoryMock.Verify(repo => repo.GetByIdAsync(appointment.Id), Times.Once);
        _appointmentResultRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<AppointmentResultEntity>()), Times.Never);
    }

    [Test]
    public async Task DeleteAppointmentResultAsync_ShouldDeleteResult()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        _appointmentResultRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(appointmentResult);
        _appointmentResultRepositoryMock.Setup(repo => repo.DeleteAsync(appointmentResult)).Returns(Task.CompletedTask);

        // Act
        await _appointmentResultService.DeleteAppointmentResultAsync(id);

        // Assert
        _appointmentResultRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _appointmentResultRepositoryMock.Verify(repo => repo.DeleteAsync(appointmentResult), Times.Once);
    }
}