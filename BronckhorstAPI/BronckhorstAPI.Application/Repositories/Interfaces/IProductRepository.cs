using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Pagination;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Application.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<PageResult<Product>> GetProductsByFiltersAsync(ProductFilters productFilters);
}