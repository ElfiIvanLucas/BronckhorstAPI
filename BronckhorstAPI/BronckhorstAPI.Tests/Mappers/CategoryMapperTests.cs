using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Tests.Mappers;

public class CategoryMapperTests
{
    [Fact]
    public void TestMapToDto_CategoryEntity_ReturnsCategoryDto()
    {
        // Arrange
        IMapper<Category, CategoryDto> categoryMapper = new CategoryMapper();
        var category = new Category
        {
            Id = 1,
            Name = "Category",
            TranslatedName = "Categorie",
            ParentId = 2
        };

        // Act
        var result = categoryMapper.MapToDto(category);

        // Assert
        Assert.Equivalent(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            TranslatedName = category.TranslatedName,
            ParentId = category.ParentId
        }, result);
    }
}