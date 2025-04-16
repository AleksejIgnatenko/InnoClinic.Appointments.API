using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;

namespace InnoClinic.Appointments.TestSuiteNUnit.ServiceTests;

class ValidationServiceTests
{
    private ValidationService _validationService;

    [SetUp]
    public void SetUp()
    {
        _validationService = new ValidationService();
    }

    [Test]
    public void ValidateAppointment_ValidAppointment_ReturnsNoErrors()
    {
        // Arrange
        var appointment = new AppointmentEntity
        {
            Id = Guid.NewGuid(),
            Date = "2025-01-01",
            Time = "08:00 - 08:10",
            IsApproved = true,
        };

        // Act
        Dictionary<string, string> errors = _validationService.Validation(appointment);

        // Assert
        Assert.That(errors.Count, Is.EqualTo(0));
    }

    [Test]
    public void ValidateAppointment_InvalidAppointment_ReturnsErrors()
    {
        // Arrange
        var appointment = new AppointmentEntity();

        // Act
        Dictionary<string, string> errors = _validationService.Validation(appointment);

        // Assert
        Assert.That(errors.Count, Is.GreaterThan(0));
    }

    [Test]
    public void ValidateAppointmentResult_ValidAppointmentResult_ReturnsNoErrors()
    {
        // Arrange
        var appointmentResult = new AppointmentResultEntity
        {
            Complaints = "Complaints",
            Conclusion = "Conclusion",
            Recommendations = "Recommendations",
            Diagnosis = "Diagnosis"
        };

        // Act
        Dictionary<string, string> errors = _validationService.Validation(appointmentResult);

        // Assert
        Assert.That(errors.Count, Is.EqualTo(0));
    }

    [Test]
    public void ValidateAppointmentResult_InvalidAppointmentResult_ReturnsErrors()
    {
        // Arrange
        var appointmentResult = new AppointmentResultEntity();

        // Act
        Dictionary<string, string> errors = _validationService.Validation(appointmentResult);

        // Assert
        Assert.That(errors.Count, Is.GreaterThan(0));
    }
}
