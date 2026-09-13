using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Application.Pagination;
using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Application.Services.Interfaces;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Application.Services;

public class ProductService : IProductService
{
    private readonly IMapper<Product, ProductDto> _productMapper;
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository, IMapper<Product, ProductDto> productMapper)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _productMapper = productMapper ?? throw new ArgumentNullException(nameof(productMapper));
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var productEntities = await _productRepository.GetAllAsync();

        return [.. productEntities.Select(_productMapper.MapToDto)];
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var productEntity = await _productRepository.GetByIdAsync(id);

        return _productMapper.MapToDto(productEntity);
    }

    public async Task<PageResult<ProductDto>> GetProductsByFiltersAsync(ProductFilters productFilters)
    {
        var result = await _productRepository.GetProductsByFiltersAsync(productFilters);

        return new PageResult<ProductDto>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(_productMapper.MapToDto).ToList()
        };
    }
}
