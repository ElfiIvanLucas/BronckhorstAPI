using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Pagination;

namespace BronckhorstAPI.Application.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();

    Task<PageResult<ProductDto>> GetProductsByFiltersAsync(ProductFilters productFilters);

    Task<PageResult<ProductGroupDto>> GetProductGroupsAsync(ProductFilters productFilters);

    Task<List<ProductDto>> GetProductsByCodeAsync(string productCode);

    Task<ProductDto> GetProductByIdAsync(int id);
}
