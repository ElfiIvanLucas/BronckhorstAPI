using System.Formats.Asn1;
using BronckhorstAPI.Controller;
using BronckhorstAPI.DTO;
using BronckhorstAPI.Services.Interfaces;
using BronckhorstAPI.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;

namespace BronckhorstAPI.Tests.Controllers;

public class ProductControllerTests
{
    private readonly IProductService _productService;
    private readonly ProductController _productController;

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
        Assert.Equal(expectedProduct, actualProduct);
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
        Assert.Equal(expectedProducts, actualProducts);
        await _productService.Received(1).GetAllProductsAsync();
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
    public async Task TestGetProducts_InternalServerError_ReturnsOkResult()
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