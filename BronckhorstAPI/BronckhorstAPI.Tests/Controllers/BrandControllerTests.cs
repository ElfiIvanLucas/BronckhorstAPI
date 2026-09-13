using BronckhorstAPI.Application.DTO;
using BronckhorstAPI.Application.Services.Interfaces;
using BronckhorstAPI.Controller;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;

namespace BronckhorstAPI.Tests.Controllers;

public class BrandControllerTests
{
    [Fact]
    public void TestConstructor_NullParameters_ThrowsArgumentNullException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BrandController(null!));
    }

    [Theory]
    [MemberData(nameof(BrandResults))]
    public async Task TestGetProducts_BrandResults_ReturnsExpectedResponse(List<BrandDto> brands, Type expectedResultType)
    {
        // Arrange
        var brandService = Substitute.For<IBrandService>();
        brandService.GetAllBrandsAsync().Returns(brands);
        var brandController = new BrandController(brandService);

        // Act
        var result = await brandController.GetProducts();

        // Assert
        Assert.IsType(expectedResultType, result);
        await brandService.Received(1).GetAllBrandsAsync();
    }

    [Fact]
    public async Task TestGetProducts_ServiceThrowsException_ReturnsStatusCode500()
    {
        // Arrange
        var brandService = Substitute.For<IBrandService>();
        brandService.GetAllBrandsAsync().ThrowsAsync(new Exception());
        var brandController = new BrandController(brandService);

        // Act
        var result = await brandController.GetProducts();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        await brandService.Received(1).GetAllBrandsAsync();
    }

    public static TheoryData<List<BrandDto>, Type> BrandResults =>
    [
        ([], typeof(NotFoundResult)),
        ([new BrandDto { Id = 1, Name = "Bronckhorst" }], typeof(OkObjectResult))
    ];
}