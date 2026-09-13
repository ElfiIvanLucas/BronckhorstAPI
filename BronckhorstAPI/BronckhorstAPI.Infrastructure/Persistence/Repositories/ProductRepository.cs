using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Pagination;
using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BronckhorstAPI.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly BronckhorstDbContext _context;

    public ProductRepository(BronckhorstDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Product>> GetAllAsync()
        => await _context.Products.ToListAsync();

    public async Task<PageResult<Product>> GetProductsByFiltersAsync(ProductFilters productFilters)
    {
        var result = _context.Products.AsQueryable();
        result = FilterProducts(result, productFilters);

        var itemsToSkip = (productFilters.Page - 1) * productFilters.PageSize;
        var totalCount = await result.CountAsync();
        var items = await result.Skip(itemsToSkip)
            .Take(productFilters.PageSize)
            .ToListAsync();

        return new PageResult<Product>
        {
            Page = productFilters.Page,
            PageSize = productFilters.PageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var result = await _context.Products.FindAsync(id);

        return result ?? throw new ArgumentException($"Product with id {id} does not exist.");
    }

    private static IQueryable<Product> FilterProducts(IQueryable<Product> products, ProductFilters productFilters)
    {
        if (productFilters.BrandId.HasValue)
        {
            products = products.Where(p => p.BrandId == productFilters.BrandId.Value);
        }

        if (productFilters.CategoryId.HasValue)
        {
            products = products.Where(p =>
                p.ProductCategories.FirstOrDefault(c => c.CategoryId == productFilters.CategoryId.Value) != null);
        }

        if (productFilters.MinPrice.HasValue)
        {
            products = products.Where(p => p.Price >= productFilters.MinPrice.Value);
        }

        if (productFilters.MaxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= productFilters.MaxPrice.Value);
        }

        if (!string.IsNullOrEmpty(productFilters.Name))
        {
            products = products.Where(p => p.Name.Contains(productFilters.Name));
        }

        return SortProducts(products, productFilters.Sort);
    }

    private static IQueryable<Product> SortProducts(IQueryable<Product> products, ProductSort sort) =>
        sort switch
        {
            ProductSort.PriceAsc => products.OrderBy(p => p.Price),
            ProductSort.PriceDesc => products.OrderByDescending(p => p.Price),
            ProductSort.NameAsc => products.OrderBy(p => p.Name),
            ProductSort.NameDesc => products.OrderByDescending(p => p.Name),
            _ => products
        };
}
