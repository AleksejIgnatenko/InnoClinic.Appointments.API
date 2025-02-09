
namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}