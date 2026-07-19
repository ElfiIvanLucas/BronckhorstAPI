using Domain.Entities;

namespace Persistence.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();
    
    Task<Product> GetByIdAsync(int id);
}