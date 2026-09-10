using BronckhorstAPI.Controller;
using BronckhorstAPI.DTO;
using BronckhorstAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;

namespace BronckhorstAPI.Tests.Controllers;

public class CategoryControllerTests
{
    private readonly CategoryController _categoryController;
    private readonly ICategoryService _categoryService;

    public CategoryControllerTests()
    {
        _categoryService = Substitute.For<ICategoryService>();
        _categoryController = new CategoryController(_categoryService);
    }

    [Fact]
    public void TestConstructor_NullParameters_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new CategoryController(null!));
    }

    [Fact]
    public async Task TestGetCategories_HasCategories_ReturnsOkResult()
    {
        // Arrange
        var expectedCategories = new List<CategoryDto> { new() { Id = 1, Name = "Category" } };
        _categoryService.GetAllCategoriesAsync().Returns(expectedCategories);

        // Act
        var result = await _categoryController.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualCategories = Assert.IsType<List<CategoryDto>>(okResult.Value);
        Assert.Equivalent(expectedCategories, actualCategories);
        await _categoryService.Received(1).GetAllCategoriesAsync();
    }

    [Fact]
    public async Task TestGetCategories_HasNoCategories_ReturnsNotFoundResult()
    {
        // Arrange
        _categoryService.GetAllCategoriesAsync().Returns([]);

        // Act
        var result = await _categoryController.GetCategories();

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await _categoryService.Received(1).GetAllCategoriesAsync();
    }

    [Fact]
    public async Task TestGetCategories_InternalServerError_ReturnsStatusCode500()
    {
        // Arrange
        _categoryService.GetAllCategoriesAsync().ThrowsAsync(new Exception());

        // Act
        var result = await _categoryController.GetCategories();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        await _categoryService.Received(1).GetAllCategoriesAsync();
    }
}
