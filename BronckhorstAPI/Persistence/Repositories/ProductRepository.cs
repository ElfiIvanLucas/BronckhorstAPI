using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Interfaces;

namespace Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly BronckhorstDbContext _context;

    public ProductRepository(BronckhorstDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<List<Product>> GetAllAsync()
        => _context.Products.ToListAsync();

    public Task<List<Product>> GetByCategoryIdAsync(int categoryId)
        => _context.Products
            .Where(product =>
                product.ProductCategories.Any(productCategory => productCategory.CategoryId == categoryId))
            .ToListAsync();

    public async Task<Product> GetByIdAsync(int id)
    {
        var result = await _context.Products.FindAsync(id);

        return result ?? throw new ArgumentException($"Product with id {id} does not exist.");
    }
}
