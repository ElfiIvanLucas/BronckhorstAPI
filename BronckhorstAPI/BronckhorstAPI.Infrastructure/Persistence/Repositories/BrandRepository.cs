using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BronckhorstAPI.Infrastructure.Persistence.Repositories;

public class BrandRepository : IRepository<Brand>
{
    private readonly BronckhorstDbContext _context;

    public BrandRepository(BronckhorstDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Brand>> GetAllAsync() =>
        await _context.Brands.ToListAsync();

    public async Task<Brand> GetByIdAsync(int id)
    {
        var result = await _context.Brands.FindAsync(id);

        return result ?? throw new ArgumentException($"Brand with id {id} does not exist.");
    }
}
