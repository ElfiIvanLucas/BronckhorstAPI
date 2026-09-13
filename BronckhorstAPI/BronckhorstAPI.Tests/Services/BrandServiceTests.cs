using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Mappers.Interfaces;
using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Application.Services;
using BronckhorstAPI.Domain.Entities;

namespace BronckhorstAPI.Tests.Services;

public class BrandServiceTests
{
    [Fact]
    public void TestConstructor_NullParameters_ThrowsArgumentNullException()
    {
        // Arrange
        var brandRepository = Substitute.For<IRepository<Brand>>();
        var brandMapper = Substitute.For<IMapper<Brand, BrandDto>>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BrandService(null!, brandMapper));
        Assert.Throws<ArgumentNullException>(() => new BrandService(brandRepository, null!));
    }

    [Fact]
    public async Task TestGetAllBrandsAsync_HasBrands_ReturnsMappedBrands()
    {
        // Arrange
        var brandRepository = Substitute.For<IRepository<Brand>>();
        var brandMapper = Substitute.For<IMapper<Brand, BrandDto>>();
        var brand = new Brand { Id = 1, Name = "Bronckhorst" };
        var expectedBrand = new BrandDto { Id = brand.Id, Name = brand.Name };
        brandRepository.GetAllAsync().Returns([brand]);
        brandMapper.MapToDto(brand).Returns(expectedBrand);
        var brandService = new BrandService(brandRepository, brandMapper);

        // Act
        var result = await brandService.GetAllBrandsAsync();

        // Assert
        Assert.Equivalent(new List<BrandDto> { expectedBrand }, result);
        await brandRepository.Received(1).GetAllAsync();
        brandMapper.Received(1).MapToDto(brand);
    }
}