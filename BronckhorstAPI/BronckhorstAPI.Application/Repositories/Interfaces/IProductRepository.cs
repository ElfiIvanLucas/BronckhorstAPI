using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Pagination;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Application.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<PageResult<Product>> GetProductsByFiltersAsync(ProductFilters productFilters);
    Task<PageResult<ProductGroupDto>> GetProductGroupsAsync(ProductFilters productFilters);
    Task<List<Product>> GetProductsByCodeAsync(string productCode);
}
