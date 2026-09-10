using Domain.Entities;

namespace Persistence.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetByCategoryIdAsync(int categoryId);
}
