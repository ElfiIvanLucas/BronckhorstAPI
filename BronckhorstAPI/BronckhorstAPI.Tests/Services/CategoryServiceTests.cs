using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Application.Services;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Tests.Services;

public class CategoryServiceTests
{
    [Fact]
    public void TestConstructor_NullParameters_ThrowsArgumentNullException()
    {
        // Arrange
        var categoryRepository = Substitute.For<IRepository<Category>>();
        var categoryMapper = Substitute.For<IMapper<Category, CategoryDto>>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CategoryService(null!, categoryMapper));
        Assert.Throws<ArgumentNullException>(() => new CategoryService(categoryRepository, null!));
    }

    [Fact]
    public async Task TestGetAllCategoriesAsync_HasCategories_ReturnsMappedCategories()
    {
        // Arrange
        var categoryRepository = Substitute.For<IRepository<Category>>();
        var categoryMapper = Substitute.For<IMapper<Category, CategoryDto>>();
        var category = new Category { Id = 1, Name = "Category" };
        var expectedCategory = new CategoryDto { Id = category.Id, Name = category.Name };
        categoryRepository.GetAllAsync().Returns([category]);
        categoryMapper.MapToDto(category).Returns(expectedCategory);
        var categoryService = new CategoryService(categoryRepository, categoryMapper);

        // Act
        var result = await categoryService.GetAllCategoriesAsync();

        // Assert
        Assert.Equivalent(new List<CategoryDto> { expectedCategory }, result);
        await categoryRepository.Received(1).GetAllAsync();
        categoryMapper.Received(1).MapToDto(category);
    }
}