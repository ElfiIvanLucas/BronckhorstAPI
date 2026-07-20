using BronckhorstAPI.DTO;
using BronckhorstAPI.Mappers.Interfaces;
using BronckhorstAPI.Services;
using BronckhorstAPI.Services.Interfaces;
using BronckhorstAPI.Tests.Helpers;
using Domain.Entities;
using Persistence.Repositories.Interfaces;

namespace BronckhorstAPI.Tests.Services;

public class ProductServiceTests
{
    private readonly IRepository<Product> _substituteProductRepository;
    private readonly IMapper<Product, ProductDto> _substituteProductMapper;
    private readonly IProductService _productService;

    public ProductServiceTests()
    {
        _substituteProductRepository = Substitute.For<IRepository<Product>>();
        _substituteProductMapper = Substitute.For<IMapper<Product, ProductDto>>();
        _productService = new ProductService(_substituteProductRepository, _substituteProductMapper);
    }

    [Fact]
    public void TestConstructor_NullParameters_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ProductService(null!, _substituteProductMapper));
        
        Assert.Throws<ArgumentNullException>(() => new ProductService(_substituteProductRepository, null!));
    }

    [Fact]
    public async Task TestGetProductByIdAsync()
    {
        // Arrange
        const int productId = 1;
        var expectedResult = ProductHelper.CreateProductDto();
        _substituteProductRepository.GetByIdAsync(productId).Returns(ProductHelper.CreateProductEntity());
        _substituteProductMapper.MapToDto(Arg.Any<Product>()).Returns(expectedResult);
        
        // Act
        var result = await _productService.GetProductByIdAsync(productId);
        
        // Assert
        Assert.Equal(expectedResult, result);
        _substituteProductMapper.Received(1).MapToDto(Arg.Any<Product>());
        await _substituteProductRepository.Received(1).GetByIdAsync(productId);
    }
    
    [Fact]
    public async Task TestGetAllProductsAsync_HasValues_ReturnsAllProducts()
    {
        // Arrange
        var expectedResult = new List<ProductDto> { ProductHelper.CreateProductDto() };
        _substituteProductRepository.GetAllAsync().Returns(ProductHelper.CreateProductEntities());
        _substituteProductMapper.MapToDto(Arg.Any<Product>()).Returns(ProductHelper.CreateProductDto());
        
        // Act
        var result = await _productService.GetAllProductsAsync();
        
        // Assert
        Assert.Equivalent(expectedResult, result);
        _substituteProductMapper.Received(1).MapToDto(Arg.Any<Product>());
        await _substituteProductRepository.Received(1).GetAllAsync();
    }
}