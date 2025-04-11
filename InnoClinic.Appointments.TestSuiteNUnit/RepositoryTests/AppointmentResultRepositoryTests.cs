using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;
using InnoClinic.Appointments.DataAccess.Context;
using InnoClinic.Appointments.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace InnoClinic.Appointments.TestSuiteNUnit.RepositoryTests;

class AppointmentResultRepositoryTests
{
    private PostgreSqlContainer _dbContainer;
    private InnoClinicAppointmentsDbContext _context;
    private AppointmentResultRepository _repository;

    private AppointmentResultEntity appointmentResult;
    private AppointmentResultEntity appointmentResult1;

    public AppointmentResultRepositoryTests()
    {
        appointmentResult = new AppointmentResultEntity
        {
            Id = Guid.NewGuid(),
            Complaints = "Complaints",
            Conclusion = "Conclusion",
            Recommendations = "Recommendations",
            Diagnosis = "Diagnosis",
            Appointment = new AppointmentEntity(),
        };

        appointmentResult1 = new AppointmentResultEntity
        {
            Id = Guid.NewGuid(),
            Complaints = "Complaints1",
            Conclusion = "Conclusion1",
            Recommendations = "Recommendations1",
            Diagnosis = "Diagnosis1",
            Appointment = new AppointmentEntity(),
        };
    }

    [SetUp]
    public async Task SetUp()
    {
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
        _repository = new AppointmentResultRepository(_context);
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
    public async Task GetAllAsync_ShouldReturnAllAppointmentResults()
    {
        // Arrange
        await _context.AppointmentResults.AddAsync(appointmentResult);
        await _context.AppointmentResults.AddAsync(appointmentResult1);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnAppointmentResultById()
    {
        // Arrange
        await _context.AppointmentResults.AddAsync(appointmentResult);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(appointmentResult.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(appointmentResult.Id, result.Id);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnDataRepositoryException()
    {
        // Arrange
        var id = Guid.NewGuid();

        await _context.AppointmentResults.AddAsync(appointmentResult);
        await _context.SaveChangesAsync();

        // Act and Assert
        Assert.ThrowsAsync<DataRepositoryException>(async () => await _repository.GetByIdAsync(id));
    }

    [Test]
    public async Task GetAllByAppointmentIdAsync_ShouldReturnAllAppointmentsByAppointmentId()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var appointmentId1 = Guid.NewGuid();

        appointmentResult.Appointment.Id = appointmentId;
        appointmentResult1.Appointment.Id = appointmentId1;

        await _context.AppointmentResults.AddAsync(appointmentResult);
        await _context.AppointmentResults.AddAsync(appointmentResult1);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByAppointmentIdAsync(appointmentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());

        foreach (var appointmentResult in result)
        {
            Assert.AreEqual(appointmentId, appointmentResult.Appointment.Id);
        }
    }

    [Test]
    public async Task IsAppointmentResultsExistenceAsync_ShouldReturnTrue()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();

        appointmentResult.Appointment.Id = appointmentId;

        await _context.AppointmentResults.AddAsync(appointmentResult);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.IsAppointmentResultsExistenceAsync(appointmentId);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task IsAppointmentResultsExistenceAsync_ShouldReturnFalse()
    {
        // Arrange
        var searchAppoimentId = Guid.NewGuid();
        var appointmentId = Guid.NewGuid();

        appointmentResult.Appointment.Id = appointmentId;

        await _context.AppointmentResults.AddAsync(appointmentResult);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.IsAppointmentResultsExistenceAsync(searchAppoimentId);

        // Assert
        Assert.IsFalse(result);
    }
}
