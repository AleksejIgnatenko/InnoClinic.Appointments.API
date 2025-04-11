using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Appointments.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace InnoClinic.Appointments.TestSuiteNUnit.RepositoryTests;

class PatientRepositoryTests
{
    private PostgreSqlContainer _dbContainer;
    private InnoClinicAppointmentsDbContext _context;
    private PatientRepository _repository;

    private PatientEntity patient;

    public PatientRepositoryTests()
    {
        patient = new PatientEntity
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            MiddleName = "A",
            AccountId = Guid.NewGuid()
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
        _repository = new PatientRepository(_context);
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
    public async Task GetByIdAsync_ShouldReturnPatientById()
    {
        // Arrange
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(patient.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(patient.Id, result.Id);
        Assert.AreEqual(patient.FirstName, result.FirstName);
        Assert.AreEqual(patient.LastName, result.LastName);
        Assert.AreEqual(patient.MiddleName, result.MiddleName);
        Assert.AreEqual(patient.AccountId, result.AccountId);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnDataRepositoryException()
    {
        // Arrange
        var id = Guid.NewGuid(); 

        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act and Assert
        Assert.ThrowsAsync<DataRepositoryException>(async () => await _repository.GetByIdAsync(id));
    }

    [Test]
    public async Task GetByAccountIdAsync_ShouldReturnPatientByAccountId()
    {
        // Arrange
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByAccountIdAsync(patient.AccountId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(patient.Id, result.Id);
        Assert.AreEqual(patient.FirstName, result.FirstName);
        Assert.AreEqual(patient.LastName, result.LastName);
        Assert.AreEqual(patient.MiddleName, result.MiddleName);
        Assert.AreEqual(patient.AccountId, result.AccountId);
    }

    [Test]
    public async Task GetByAccountIdAsync_ShouldReturnDataRepositoryException()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act and Assert
        Assert.ThrowsAsync<DataRepositoryException>(async () => await _repository.GetByAccountIdAsync(accountId));
    }
}
