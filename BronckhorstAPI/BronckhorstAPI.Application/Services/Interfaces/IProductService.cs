using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Pagination;

namespace BronckhorstAPI.Application.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();

    Task<PageResult<ProductDto>> GetProductsByFiltersAsync(ProductFilters productFilters);

    Task<ProductDto> GetProductByIdAsync(int id);
}