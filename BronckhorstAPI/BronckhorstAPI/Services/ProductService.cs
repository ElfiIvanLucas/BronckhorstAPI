using BronckhorstAPI.DTO;
using BronckhorstAPI.Mappers.Interfaces;
using BronckhorstAPI.Services.Interfaces;
using Domain.Entities;
using Persistence.Repositories.Interfaces;

namespace BronckhorstAPI.Services;

public class ProductService : IProductService
{
    private readonly IMapper<Product, ProductDto> _productMapper;
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository, IMapper<Product, ProductDto> productMapper)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _productMapper = productMapper ?? throw new ArgumentNullException(nameof(productMapper));
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var productEntities = await _productRepository.GetAllAsync();

        return productEntities.Select(_productMapper.MapToDto).ToList();
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var productEntity = await _productRepository.GetByIdAsync(id);

        return _productMapper.MapToDto(productEntity);
    }
}
