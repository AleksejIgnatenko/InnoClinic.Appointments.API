using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.DataAccess.Context;
using InnoClinic.Appointments.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace InnoClinic.Appointments.TestSuiteNUnit.RepositoryTests;

class MedicalServiceRepositoryTests
{
    private PostgreSqlContainer _dbContainer;
    private InnoClinicAppointmentsDbContext _context;
    private MedicalServiceRepository _repository;

    private MedicalServiceEntity medicalService;

    [SetUp]
    public async Task SetUp()
    {
        medicalService = new MedicalServiceEntity
        {
            Id = Guid.NewGuid(),
            ServiceName = "ServiceName",
            Price = 0,
            IsActive = true,
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
        _repository = new MedicalServiceRepository(_context);
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
        await _context.MedicalServices.AddAsync(medicalService);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(medicalService.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(medicalService.Id, result.Id);
        Assert.AreEqual(medicalService.ServiceName, result.ServiceName);
        Assert.AreEqual(medicalService.Price, result.Price);
        Assert.AreEqual(medicalService.IsActive, result.IsActive);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnDataRepositoryException()
    {
        // Arrange
        var id = Guid.NewGuid();

        await _context.MedicalServices.AddAsync(medicalService);
        await _context.SaveChangesAsync();

        // Act and Assert
        Assert.ThrowsAsync<DataRepositoryException>(async () => await _repository.GetByIdAsync(id));
    }
}
