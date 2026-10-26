using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.DTO;
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
        result = SortProducts(FilterProducts(result, productFilters), productFilters.Sort);

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

    public async Task<PageResult<ProductGroupDto>> GetProductGroupsAsync(ProductFilters productFilters)
    {
        var groups = FilterProducts(_context.Products.AsNoTracking(), productFilters)
            .GroupBy(product => product.ProductCode)
            .Select(group => new ProductGroupDto
            {
                ProductCode = group.Key,
                Image = group.OrderBy(product => product.Id).Select(product => product.Image).FirstOrDefault(),
                ProductCount = group.Count(),
                FromPrice = group.Min(product => product.Price)
            });

        groups = productFilters.Sort switch
        {
            ProductSort.NameDesc => groups.OrderByDescending(group => group.ProductCode),
            ProductSort.PriceAsc => groups.OrderBy(group => group.FromPrice).ThenBy(group => group.ProductCode),
            ProductSort.PriceDesc => groups.OrderByDescending(group => group.FromPrice).ThenBy(group => group.ProductCode),
            _ => groups.OrderBy(group => group.ProductCode)
        };

        var pageSize = Math.Clamp(productFilters.PageSize, 1, 100);
        var totalCount = await groups.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));
        var page = Math.Clamp(productFilters.Page, 1, totalPages);

        return new PageResult<ProductGroupDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = await groups.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync()
        };
    }

    public Task<List<Product>> GetProductsByCodeAsync(string productCode)
        => _context.Products.AsNoTracking()
            .Where(product => product.ProductCode == productCode)
            .OrderBy(product => product.Id)
            .ToListAsync();

    private static IQueryable<Product> FilterProducts(IQueryable<Product> products, ProductFilters productFilters)
    {
        if (productFilters.BrandId.HasValue)
        {
            products = products.Where(p => p.BrandId == productFilters.BrandId.Value);
        }

        if (productFilters.CategoryId is int categoryId)
        {
            products = products.Where(product => product.ProductCategories.Any(category =>
                category.CategoryId == categoryId &&
                (!productFilters.ParentCategoryId.HasValue ||
                 category.Category.ParentId == productFilters.ParentCategoryId.Value)));
        }
        else if (productFilters.ParentCategoryId is int parentCategoryId)
        {
            products = products.Where(product => product.ProductCategories.Any(category =>
                category.CategoryId == parentCategoryId || category.Category.ParentId == parentCategoryId));
        }

        if (!string.IsNullOrEmpty(productFilters.Name))
        {
            products = products.Where(p =>
                p.Name.Contains(productFilters.Name) || p.ArtikelCode.Contains(productFilters.Name) ||
                p.ProductCode.Contains(productFilters.Name));
        }

        return products;
    }

    private static IQueryable<Product> SortProducts(IQueryable<Product> products, ProductSort sort) =>
        sort switch
        {
            ProductSort.PriceAsc => products.OrderBy(p => p.Price).ThenBy(p => p.Id),
            ProductSort.PriceDesc => products.OrderByDescending(p => p.Price).ThenBy(p => p.Id),
            ProductSort.NameAsc => products.OrderBy(p => p.Name).ThenBy(p => p.Id),
            ProductSort.NameDesc => products.OrderByDescending(p => p.Name).ThenBy(p => p.Id),
            _ => products
        };
}
