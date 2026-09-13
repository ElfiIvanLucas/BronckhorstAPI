using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Application.Pagination;
using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Application.Services;
using BronckhorstAPI.Application.Services.Interfaces;
using BronckhorstAPI.Domain.Entities;
using BronckhorstAPI.Tests.Helpers;

namespace BronckhorstAPI.Tests.Services;

public class ProductServiceTests
{
    private readonly IProductService _productService;
    private readonly IMapper<Product, ProductDto> _substituteProductMapper;
    private readonly IProductRepository _substituteProductRepository;

    public ProductServiceTests()
    {
        _substituteProductRepository = Substitute.For<IProductRepository>();
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
        Assert.Equivalent(expectedResult, result);
        _substituteProductMapper.Received(1).MapToDto(Arg.Any<Product>());
        await _substituteProductRepository.Received(1).GetByIdAsync(productId);
    }

    [Fact]
    public async Task TestGetProductsByFiltersAsync_HasValues_ReturnsFilteredProducts()
    {
        // Arrange
        var productFilters = new ProductFilters { CategoryId = 1 };
        var expectedResult = new PageResult<ProductDto>
        {
            Page = 1,
            PageSize = 10,
            TotalCount = 1,
            Items = [ProductHelper.CreateProductDto()]
        };
        _substituteProductRepository.GetProductsByFiltersAsync(productFilters)
            .Returns(new PageResult<Product>
            {
                Page = 1,
                PageSize = 10,
                TotalCount = 1,
                Items = ProductHelper.CreateProductEntities()
            });
        _substituteProductMapper.MapToDto(Arg.Any<Product>()).Returns(ProductHelper.CreateProductDto());

        // Act
        var result = await _productService.GetProductsByFiltersAsync(productFilters);

        // Assert
        Assert.Equivalent(expectedResult, result);
        _substituteProductMapper.Received(1).MapToDto(Arg.Any<Product>());
        await _substituteProductRepository.Received(1).GetProductsByFiltersAsync(productFilters);
    }
}