using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.DataAccess.Context;
using InnoClinic.Appointments.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace InnoClinic.Appointments.TestSuiteNUnit.RepositoryTests;

class DoctorRepositoryTests
{
    private PostgreSqlContainer _dbContainer;
    private InnoClinicAppointmentsDbContext _context;
    private DoctorRepository _repository;

    private DoctorEntity doctor;

    [SetUp]
    public async Task SetUp()
    {
        doctor = new DoctorEntity
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName",
            MiddleName = "MiddleName",
            CabinetNumber = 1,
            Status = "Status",
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
        _repository = new DoctorRepository(_context);
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
    public async Task GetByIdAsync_ShouldReturnDoctorById()
    {
        // Arrange
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(doctor.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(doctor.Id, result.Id);
        Assert.AreEqual(doctor.AccountId, result.AccountId);
        Assert.AreEqual(doctor.FirstName, result.FirstName);
        Assert.AreEqual(doctor.LastName, result.LastName);
        Assert.AreEqual(doctor.MiddleName, result.MiddleName);
        Assert.AreEqual(doctor.CabinetNumber, result.CabinetNumber);
        Assert.AreEqual(doctor.Status, result.Status);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnDataRepositoryException()
    {
        // Arrange
        var id = Guid.NewGuid();

        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();

        // Act and Assert
        Assert.ThrowsAsync<DataRepositoryException>(async () => await _repository.GetByIdAsync(id));
    }
}
