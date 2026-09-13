using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Tests.Mappers;

public class BrandMapperTests
{
    [Fact]
    public void TestMapToDto_BrandEntity_ReturnsBrandDto()
    {
        // Arrange
        IMapper<Brand, BrandDto> brandMapper = new BrandMapper();
        var brand = new Brand { Id = 1, Name = "Bronckhorst" };

        // Act
        var result = brandMapper.MapToDto(brand);

        // Assert
        Assert.Equivalent(new BrandDto { Id = brand.Id, Name = brand.Name }, result);
    }
}