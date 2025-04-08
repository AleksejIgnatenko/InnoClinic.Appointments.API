using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Appointments.DataAccess.Context;
using InnoClinic.Appointments.DataAccess.Repositories;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for performing CRUD operations on patient entities.
/// </summary>
public class PatientRepository : RepositoryBase<PatientEntity>, IPatientRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PatientRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PatientRepository(InnoClinicAppointmentsDbContext context) : base(context) { }

    /// <summary>
    /// Retrieves a patient entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the patient entity.</param>
    /// <returns>The patient entity if found, otherwise throws a DataRepositoryException.</returns>
    public async Task<PatientEntity> GetByIdAsync(Guid id)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new DataRepositoryException("Patient not found", 404);
    }

    /// <summary>
    /// Retrieves a patient entity by its account ID asynchronously.
    /// </summary>
    /// <param name="accountId">The account ID associated with the patient entity.</param>
    /// <returns>The patient entity if found, otherwise throws a DataRepositoryException.</returns>
    public async Task<PatientEntity> GetByAccountIdAsync(Guid accountId)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.AccountId == accountId)
            ?? throw new DataRepositoryException("Patient not found", 404);
    }
}