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

class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly Mock<IMedicalServiceRepository> _medicalServiceRepositoryMock;
    private readonly Mock<IValidationService> _validationServiceMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IAppointmentResultRepository> _appointmentResultRepositoryMock;

    private readonly AppointmentService _appointmentService;

    private AppointmentEntity appointment;
    private PatientEntity patient;
    private DoctorEntity doctor;
    private MedicalServiceEntity medicalService;

    public AppointmentServiceTests()
    {
        patient = new PatientEntity
        {
            Id = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName",
            MiddleName = "MiddleName",
            AccountId = Guid.NewGuid(),
        };

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

        medicalService = new MedicalServiceEntity
        {
            Id = Guid.NewGuid(),
            ServiceName = "ServiceName",
            Price = 1,
            IsActive = true,
        };

        appointment = new AppointmentEntity
        {
            Id = Guid.NewGuid(),
            Patient = patient,
            Doctor = doctor,
            MedicalService = medicalService,
            Date = "2025-01-01",
            Time = "08:00 - 08:10",
            IsApproved = true,
        };

        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _medicalServiceRepositoryMock = new Mock<IMedicalServiceRepository>();
        _validationServiceMock = new Mock<IValidationService>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _appointmentResultRepositoryMock = new Mock<IAppointmentResultRepository>();

        _appointmentService = new AppointmentService(
            _appointmentRepositoryMock.Object,
            _patientRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            _medicalServiceRepositoryMock.Object,
            _validationServiceMock.Object,
            _jwtTokenServiceMock.Object,
            _appointmentResultRepositoryMock.Object);
    }

    [Test]
    public async Task CreateAppointmentAsync_ValidInput_ShouldCreateAppointment()
    {
        // Arrange
        _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(patient.Id)).ReturnsAsync(patient);
        _doctorRepositoryMock.Setup(repo => repo.GetByIdAsync(doctor.Id)).ReturnsAsync(doctor);
        _medicalServiceRepositoryMock.Setup(repo => repo.GetByIdAsync(medicalService.Id)).ReturnsAsync(medicalService);

        _validationServiceMock.Setup(service => service.Validation(It.IsAny<AppointmentEntity>())).Returns(new Dictionary<string, string>());

        // Act
        await _appointmentService.CreateAppointmentAsync(patient.Id, doctor.Id, medicalService.Id, appointment.Date, appointment.Time, appointment.IsApproved);

        // Assert
        _appointmentRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<AppointmentEntity>(appointmentEntity =>
            appointmentEntity.Patient == patient &&
            appointmentEntity.Doctor == doctor &&
            appointmentEntity.MedicalService == medicalService &&
            appointmentEntity.Date == appointment.Date &&
            appointmentEntity.Time == appointment.Time &&
            appointmentEntity.IsApproved == appointment.IsApproved)), Times.Once);
    }

    [Test]
    public void CreateAppointmentAsync_ValidationErrors_ShouldThrowValidationException()
    {
        // Arrange
        var validationErrors = new Dictionary<string, string>
        {
            { "Date", "Date must be in the format yyyy-MM-dd." }
        };

        _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(patient.Id)).ReturnsAsync(patient);
        _doctorRepositoryMock.Setup(repo => repo.GetByIdAsync(doctor.Id)).ReturnsAsync(doctor);
        _medicalServiceRepositoryMock.Setup(repo => repo.GetByIdAsync(medicalService.Id)).ReturnsAsync(medicalService);

        _validationServiceMock.Setup(service => service.Validation(It.IsAny<AppointmentEntity>())).Returns(validationErrors);

        // Act and Assert
        ValidationException exception = Assert.ThrowsAsync<ValidationException>(async () =>
            await _appointmentService.CreateAppointmentAsync(patient.Id, doctor.Id, medicalService.Id, "", appointment.Time, appointment.IsApproved));

        Assert.NotNull(exception);
        Assert.AreEqual(validationErrors, exception.Errors);

        _patientRepositoryMock.Verify(repo => repo.GetByIdAsync(patient.Id), Times.Once);
        _doctorRepositoryMock.Verify(repo => repo.GetByIdAsync(doctor.Id), Times.Once);
        _medicalServiceRepositoryMock.Verify(repo => repo.GetByIdAsync(medicalService.Id), Times.Once);

        _appointmentResultRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<AppointmentResultEntity>()), Times.Never);
    }

    [Test]
    public async Task GetAllAppointmentsAsync_ShouldReturnAllAppointments()
    {
        // Arrange
        var appointments = new List<AppointmentEntity>
        {
            new AppointmentEntity { Id = Guid.NewGuid(), Date = "2023-10-01", Time = "10:00" },
            new AppointmentEntity { Id = Guid.NewGuid(), Date = "2023-10-02", Time = "11:00" }
        };

        _appointmentRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(appointments);

        // Act
        var results = await _appointmentService.GetAllAppointmentsAsync();

        // Assert
        Assert.NotNull(results);
        Assert.AreEqual(appointments.Count, results.Count());

        _appointmentRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetAppointmentsByDateAsync_ShouldReturnAllAppointmentsByDate()
    {
        // Arrange
        var date = "2023-10-01";
        var appointments = new List<AppointmentEntity>
        {
            new AppointmentEntity { Id = Guid.NewGuid(), Date = "2023-10-01", Time = "08:00 - 08:10" },
        };

        _appointmentRepositoryMock.Setup(repo => repo.GetByDateAsync(date)).ReturnsAsync(appointments);

        // Act
        var results = await _appointmentService.GetAppointmentsByDateAsync(date);

        // Assert
        Assert.NotNull(results);
        Assert.AreEqual(appointments.Count, results.Count());

        _appointmentRepositoryMock.Verify(repo => repo.GetByDateAsync(date), Times.Once);
    }

    [Test]
    public async Task GetAllAvailableTimeSlotsAsync_ShouldReturnAvailableTimeSlots()
    {
        // Arrange
        var date = "2023-10-01";
        var timeSlotSize = 10;
        var appointments = new List<AppointmentEntity>
        {
            new AppointmentEntity { Id = Guid.NewGuid(), Doctor = doctor, Date = "2023-10-01", Time = "08:00 - 08:10" },
            new AppointmentEntity { Id = Guid.NewGuid(), Doctor = doctor, Date = "2023-10-01", Time = "08:10 - 08:20" },
        };

        _appointmentRepositoryMock.Setup(repo => repo.GetByDateAndDoctorIdAsync(date, doctor.Id)).ReturnsAsync(appointments);

        // Act
        var results = await _appointmentService.GetAllAvailableTimeSlotsAsync(date, timeSlotSize, doctor.Id);

        // Assert
        Assert.NotNull(results);

        _appointmentRepositoryMock.Verify(repo => repo.GetByDateAndDoctorIdAsync(date, doctor.Id), Times.Once);
    }

    [Test]
    public async Task GetAllAppointmentsByPatientIdAsync_ShouldReturnAllAppointmentsByPatientId()
    {
        // Arrange
        var appointments = new List<AppointmentEntity>
        {
            new AppointmentEntity { Id = Guid.NewGuid(), Patient = patient, Date = "2023-10-01", Time = "08:00 - 08:10" },
            new AppointmentEntity { Id = Guid.NewGuid(), Patient = patient, Date = "2023-10-01", Time = "08:10 - 08:20" },
        };

        _appointmentRepositoryMock.Setup(repo => repo.GetAllByPatientIdAsync(patient.Id)).ReturnsAsync(appointments);

        // Act
        var results = await _appointmentService.GetAllAppointmentsByPatientIdAsync(patient.Id);

        // Assert
        Assert.NotNull(results);
        Assert.AreEqual(appointments.Count, results.Count());

        _appointmentRepositoryMock.Verify(repo => repo.GetAllByPatientIdAsync(patient.Id), Times.Once);
    }

    [Test]
    public async Task IsAppointmentResultsExistenceAsync_ShouldReturnTrue()
    {
        // Arrange
        _appointmentResultRepositoryMock.Setup(repo => repo.IsAppointmentResultsExistenceAsync(appointment.Id)).ReturnsAsync(true);

        // Act
        var results = await _appointmentService.IsAppointmentResultsExistenceAsync(appointment.Id);

        // Assert
        Assert.NotNull(results);
        Assert.IsTrue(results);

        _appointmentResultRepositoryMock.Verify(repo => repo.IsAppointmentResultsExistenceAsync(appointment.Id), Times.Once);
    }

    [Test]
    public async Task ГзвфеуAppointmentAsync_ValidInput_ShouldCreateAppointment()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(appointment.Id)).ReturnsAsync(appointment);
        _doctorRepositoryMock.Setup(repo => repo.GetByIdAsync(doctor.Id)).ReturnsAsync(doctor);
        _medicalServiceRepositoryMock.Setup(repo => repo.GetByIdAsync(medicalService.Id)).ReturnsAsync(medicalService);

        _validationServiceMock.Setup(service => service.Validation(It.IsAny<AppointmentEntity>())).Returns(new Dictionary<string, string>());

        // Act
        await _appointmentService.UpdateAppointmentAsync(appointment.Id, doctor.Id, medicalService.Id, appointment.Date, appointment.Time, appointment.IsApproved);

        // Assert
        _appointmentRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<AppointmentEntity>(appointmentEntity =>
            appointmentEntity.Patient == patient &&
            appointmentEntity.Doctor == doctor &&
            appointmentEntity.MedicalService == medicalService &&
            appointmentEntity.Date == appointment.Date &&
            appointmentEntity.Time == appointment.Time &&
            appointmentEntity.IsApproved == appointment.IsApproved)), Times.Once);
    }

    [Test]
    public void UpdateAppointmentAsync_ValidationErrors_ShouldThrowValidationException()
    {
        // Arrange
        var validationErrors = new Dictionary<string, string>
        {
            { "Date", "Date must be in the format yyyy-MM-dd." }
        };

        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(appointment.Id)).ReturnsAsync(appointment);
        _doctorRepositoryMock.Setup(repo => repo.GetByIdAsync(doctor.Id)).ReturnsAsync(doctor);
        _medicalServiceRepositoryMock.Setup(repo => repo.GetByIdAsync(medicalService.Id)).ReturnsAsync(medicalService);

        _validationServiceMock.Setup(service => service.Validation(It.IsAny<AppointmentEntity>())).Returns(validationErrors);

        // Act and Assert
        ValidationException exception = Assert.ThrowsAsync<ValidationException>(async () =>
            await _appointmentService.UpdateAppointmentAsync(appointment.Id, doctor.Id, medicalService.Id, "", appointment.Time, appointment.IsApproved));

        Assert.NotNull(exception);
        Assert.AreEqual(validationErrors, exception.Errors);

        _appointmentRepositoryMock.Verify(repo => repo.GetByIdAsync(appointment.Id), Times.Once);
        _doctorRepositoryMock.Verify(repo => repo.GetByIdAsync(doctor.Id), Times.Once);
        _medicalServiceRepositoryMock.Verify(repo => repo.GetByIdAsync(medicalService.Id), Times.Once);

        _appointmentResultRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<AppointmentResultEntity>()), Times.Never);
    }

    [Test]
    public async Task DeleteAppointmentAsync_ShouldDeleteResult()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(appointment.Id)).ReturnsAsync(appointment);

        // Act
        await _appointmentService.DeleteAppointmentAsync(appointment.Id);

        // Assert
        _appointmentRepositoryMock.Verify(repo => repo.GetByIdAsync(appointment.Id), Times.Once);
        _appointmentRepositoryMock.Verify(repo => repo.DeleteAsync(appointment), Times.Once);
    }
}
