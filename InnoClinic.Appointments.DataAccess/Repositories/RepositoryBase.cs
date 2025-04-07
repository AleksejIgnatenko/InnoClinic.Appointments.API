using InnoClinic.Appointments.DataAccess.Context;

namespace InnoClinic.Appointments.DataAccess.Repositories;

/// <summary>
/// Base repository class for CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly InnoClinicAppointmentsDbContext _context;

    public RepositoryBase(InnoClinicAppointmentsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public virtual async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public virtual async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}