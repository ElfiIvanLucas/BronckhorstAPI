using BronckhorstAPI.Controller;
using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Pagination;
using BronckhorstAPI.Application.Services.Interfaces;
using BronckhorstAPI.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;

namespace BronckhorstAPI.Tests.Controllers;

public class ProductControllerTests
{
    private readonly ProductController _productController;
    private readonly IProductService _productService;

    public ProductControllerTests()
    {
        _productService = Substitute.For<IProductService>();
        _productController = new ProductController(_productService);
    }

    [Fact]
    public void TestConstructor_NullParameters_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ProductController(null!));
    }

    [Fact]
    public async Task TestGetProductById_InternalServerError_ReturnsStatusCode500()
    {
        // Arrange
        const int productId = 99;
        _productService.GetProductByIdAsync(productId).ThrowsAsync(new Exception());

        // Act
        var result = await _productController.GetProductById(productId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        await _productService.Received(1).GetProductByIdAsync(productId);
    }

    [Fact]
    public async Task TestGetProductById_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        const int productId = 99;
        const string? expectedMessage = "Product not found";
        _productService.GetProductByIdAsync(productId).ThrowsAsync(new ArgumentException(expectedMessage));

        // Act
        var result = await _productController.GetProductById(productId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(expectedMessage, badRequestResult.Value);
        await _productService.Received(1).GetProductByIdAsync(productId);
    }

    [Fact]
    public async Task TestGetProductById_ValidId_ReturnsProduct()
    {
        // Arrange
        const int productId = 1;
        var expectedProduct = ProductHelper.CreateProductDto();
        _productService.GetProductByIdAsync(productId).Returns(expectedProduct);

        // Act
        var result = await _productController.GetProductById(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualProduct = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equivalent(expectedProduct, actualProduct);
        await _productService.Received(1).GetProductByIdAsync(productId);
    }

    [Fact]
    public async Task TestGetProductsByFiltersAsync_HasNoProducts_ReturnsEmptyPageResult()
    {
        // Arrange
        var productFilters = new ProductFilters { CategoryId = 1 };
        var expectedResult = new PageResult<ProductDto> { Items = [] };
        _productService.GetProductsByFiltersAsync(productFilters).Returns(expectedResult);

        // Act
        var result = await _productController.GetProductsByFiltersAsync(productFilters);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equivalent(expectedResult, Assert.IsType<PageResult<ProductDto>>(okResult.Value));
        await _productService.Received(1).GetProductsByFiltersAsync(productFilters);
    }

    [Fact]
    public async Task TestGetProductsByFiltersAsync_HasProducts_ReturnsOkResult()
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
        _productService.GetProductsByFiltersAsync(productFilters).Returns(expectedResult);

        // Act
        var result = await _productController.GetProductsByFiltersAsync(productFilters);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualProducts = Assert.IsType<PageResult<ProductDto>>(okResult.Value);
        Assert.Equivalent(expectedResult, actualProducts);
        await _productService.Received(1).GetProductsByFiltersAsync(productFilters);
    }

    [Fact]
    public async Task TestGetProductsByFiltersAsync_InternalServerError_ReturnsStatusCode500()
    {
        // Arrange
        var productFilters = new ProductFilters { CategoryId = 1 };
        _productService.GetProductsByFiltersAsync(productFilters).ThrowsAsync(new Exception());

        // Act
        var result = await _productController.GetProductsByFiltersAsync(productFilters);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        await _productService.Received(1).GetProductsByFiltersAsync(productFilters);
    }

    [Fact]
    public async Task TestGetProductsByFiltersAsync_ServiceThrowsArgumentException_ReturnsBadRequest()
    {
        // Arrange
        var productFilters = new ProductFilters { CategoryId = 1 };
        const string expectedMessage = "Invalid filters";
        _productService.GetProductsByFiltersAsync(productFilters)
            .ThrowsAsync(new ArgumentException(expectedMessage));

        // Act
        var result = await _productController.GetProductsByFiltersAsync(productFilters);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(expectedMessage, badRequestResult.Value);
        await _productService.Received(1).GetProductsByFiltersAsync(productFilters);
    }

    [Fact]
    public async Task TestGetProducts_HasNoProducts_ReturnsOkResult()
    {
        // Arrange
        var expectedProducts = new List<ProductDto>();
        _productService.GetAllProductsAsync().Returns(expectedProducts);

        // Act
        var result = await _productController.GetProducts();

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await _productService.Received(1).GetAllProductsAsync();
    }

    [Fact]
    public async Task TestGetProducts_HasProducts_ReturnsOkResult()
    {
        // Arrange
        var expectedProducts = new List<ProductDto> { ProductHelper.CreateProductDto() };
        _productService.GetAllProductsAsync().Returns(expectedProducts);

        // Act
        var result = await _productController.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualProducts = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.Equivalent(expectedProducts, actualProducts);
        await _productService.Received(1).GetAllProductsAsync();
    }

    [Fact]
    public async Task TestGetProducts_InternalServerError_ReturnsStatusCode500()
    {
        // Arrange
        _productService.GetAllProductsAsync().ThrowsAsync(new Exception());

        // Act
        var result = await _productController.GetProducts();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        await _productService.Received(1).GetAllProductsAsync();
    }
}
