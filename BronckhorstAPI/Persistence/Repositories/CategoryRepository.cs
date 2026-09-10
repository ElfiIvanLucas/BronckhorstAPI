using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Interfaces;

namespace Persistence.Repositories;

public class CategoryRepository : IRepository<Category>
{
    private readonly BronckhorstDbContext _context;

    public CategoryRepository(BronckhorstDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<List<Category>> GetAllAsync()
        => _context.Categories.ToListAsync();

    public async Task<Category> GetByIdAsync(int id)
    {
        var result = await _context.Categories.FindAsync(id);

        return result ?? throw new ArgumentException($"Category with id {id} does not exist.");
    }
}
