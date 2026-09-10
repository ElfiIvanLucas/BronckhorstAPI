namespace Persistence.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(int id);
}