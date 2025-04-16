using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Appointments.DataAccess.Context;
using InnoClinic.Appointments.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace InnoClinic.Appointments.TestSuiteNUnit.RepositoryTests;

class RepositoryBaseTests
{
    private PostgreSqlContainer _dbContainer;
    private InnoClinicAppointmentsDbContext _context;
    private RepositoryBase<AppointmentEntity> _repository;

    private AppointmentEntity appointment;

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
        _repository = new RepositoryBase<AppointmentEntity>(_context);
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
    public async Task CreateAsync_ShouldAddEntity()
    {
        // Act
        await _repository.CreateAsync(appointment);

        // Assert
        var result = await _context.Set<AppointmentEntity>().FindAsync(appointment.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(appointment.Id, result.Id);
        Assert.AreEqual(appointment.Date, result.Date);
        Assert.AreEqual(appointment.Time, result.Time);
        Assert.AreEqual(appointment.IsApproved, result.IsApproved);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        await _repository.CreateAsync(appointment);

        // Act
        appointment.Date = "2025-10-10";
        appointment.Time = "09:00 - 09:10";
        appointment.IsApproved = false;

        await _repository.UpdateAsync(appointment);

        // Assert
        var result = await _context.Set<AppointmentEntity>().FindAsync(appointment.Id);
        Assert.IsNotNull(result);
        Assert.AreEqual("2025-10-10", result.Date);
        Assert.AreEqual("09:00 - 09:10", result.Time);
        Assert.AreEqual(false, result.IsApproved);
    }

    [Test]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        await _repository.CreateAsync(appointment);

        // Act
        await _repository.DeleteAsync(appointment);

        // Assert
        var result = await _context.Set<AppointmentEntity>().FindAsync(appointment.Id);
        Assert.IsNull(result);
    }
}
