using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Appointments.DataAccess.Context;
using InnoClinic.Appointments.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace InnoClinic.Appointments.TestSuiteNUnit.RepositoryTests;

class AppointmentRepositoryTests
{
    private PostgreSqlContainer _dbContainer;
    private InnoClinicAppointmentsDbContext _context;
    private AppointmentRepository _repository;

    private AppointmentEntity appointment;
    private AppointmentEntity appointment1;

    [SetUp]
    public async Task SetUp()
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

        appointment1 = new AppointmentEntity
        {
            Id = Guid.NewGuid(),
            Patient = new PatientEntity(),
            Doctor = new DoctorEntity(),
            MedicalService = new MedicalServiceEntity(),
            Date = "2025-01-02",
            Time = "09:00 - 09:10",
            IsApproved = false,
        };

        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("TestDatabase")
            .WithUsername("postgres")
            .WithPassword("Password1!")
            .Build();

        await _dbContainer.StartAsync();

        var options = new DbContextOptionsBuilder<InnoClinicAppointmentsDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .Options;

        _context = new InnoClinicAppointmentsDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        _repository = new AppointmentRepository(_context);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllAppointments()
    {
        // Arrange
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnAppointmentById()
    {
        // Arrange
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(appointment.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(appointment.Id, result.Id);
        Assert.AreEqual(appointment .Date, result.Date);
        Assert.AreEqual(appointment.Time, result.Time);
        Assert.AreEqual(appointment.IsApproved, result.IsApproved);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnDataRepositoryException()
    {
        // Arrange
        var id = Guid.NewGuid();

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        // Act and Assert
        Assert.ThrowsAsync<DataRepositoryException>(async () => await _repository.GetByIdAsync(id));
    }

    [Test]
    public async Task GetAllByDoctorAccountIdAsync_ShouldReturnAllAppointmentsByDoctorAccountId()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        appointment.Doctor.AccountId = accountId;

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByDoctorAccountIdAsync(accountId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());

        foreach (var appointment in result)
        {
            Assert.AreEqual(accountId, appointment.Doctor.AccountId);
        }
    }

    [Test]
    public async Task GetAllByPatientIdAsync_ShouldReturnAllAppointmentsByPatientId()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        appointment.Patient.Id = patientId;

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByPatientIdAsync(patientId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());

        foreach (var appointment in result)
        {
            Assert.AreEqual(patientId, appointment.Patient.Id);
        }
    }

    [Test]
    public async Task GetByDateAsync_ShouldReturnAllAppointmentsGetByDate()
    {
        // Arrange
        string date = appointment.Date;

        await _context.Appointments.AddAsync(appointment);
        await _context.Appointments.AddAsync(appointment1);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByDateAsync(date);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());

        foreach (var appointment in result)
        {
            Assert.AreEqual(date, appointment.Date);
        }
    }

    [Test]
    public async Task GetByDateAndDoctorIdAsync_ShouldReturnAllAppointmentsGetByDateAndDoctorId()
    {
        // Arrange
        string date = appointment.Date;
        Guid doctorId = Guid.NewGuid();
        Guid doctorId1 = Guid.NewGuid();

        appointment.Doctor.Id = doctorId;
        appointment1.Doctor.Id = doctorId1;

        await _context.Appointments.AddAsync(appointment);
        await _context.Appointments.AddAsync(appointment1);

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByDateAndDoctorIdAsync(date, doctorId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());

        foreach (var appointment in result)
        {
            Assert.AreEqual(date, appointment.Date);
            Assert.AreEqual(doctorId, appointment.Doctor.Id);
        }
    }

    [Test]
    public async Task GetByAccountIdAndDateAsync_ShouldReturnAllAppointmentsGetByAccountIdAndDate()
    {
        // Arrange
        Guid accountId = Guid.NewGuid();
        Guid accountId1 = Guid.NewGuid();
        string date = appointment.Date;

        appointment.Doctor.AccountId = accountId;
        appointment1.Doctor.AccountId = accountId;

        await _context.Appointments.AddAsync(appointment);
        await _context.Appointments.AddAsync(appointment1);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByAccountIdAndDateAsync(accountId, date);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());

        foreach (var appointment in result)
        {
            Assert.AreEqual(accountId, appointment.Doctor.AccountId);
            Assert.AreEqual(date, appointment.Date);
        }
    }
}
