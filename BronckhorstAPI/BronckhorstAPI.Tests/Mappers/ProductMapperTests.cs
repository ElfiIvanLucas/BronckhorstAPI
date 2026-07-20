using BronckhorstAPI.DTO;
using BronckhorstAPI.Mappers;
using BronckhorstAPI.Mappers.Interfaces;
using BronckhorstAPI.Tests.Helpers;
using Domain.Entities;

namespace BronckhorstAPI.Tests.Mappers;

public class ProductMapperTests
{
    private readonly IMapper<Product, ProductDto> _productDtoMapper;

    public ProductMapperTests()
    {
        _productDtoMapper = new ProductMapper();
    }

    [Fact]
    public void TestMapToDto()
    {
        // Arrange
        var productEntity = ProductHelper.CreateProductEntity();
        var expectedProductDtoResult = ProductHelper.CreateProductDto();

        // Act
        var result = _productDtoMapper.MapToDto(productEntity);
        
        // Assert
        Assert.Equivalent(expectedProductDtoResult, result);
    }
}