using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BronckhorstAPI.Infrastructure.Persistence.Repositories;

public class CategoryRepository : IRepository<Category>
{
    private readonly BronckhorstDbContext _context;

    public CategoryRepository(BronckhorstDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Category>> GetAllAsync()
        => await _context.Categories.ToListAsync();

    public async Task<Category> GetByIdAsync(int id)
    {
        var result = await _context.Categories.FindAsync(id);

        return result ?? throw new ArgumentException($"Category with id {id} does not exist.");
    }
}
