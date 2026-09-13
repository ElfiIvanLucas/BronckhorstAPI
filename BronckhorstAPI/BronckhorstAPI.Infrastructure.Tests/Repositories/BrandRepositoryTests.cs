using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Domain.Entities;
using BronckhorstAPI.Infrastructure.Persistence.Repositories;
using BronckhorstAPI.Infrastructure.Tests.Helpers;

namespace BronckhorstAPI.Infrastructure.Tests.Repositories;

public class BrandRepositoryTests
{
    [Fact]
    public void TestConstructor_NullArguments_ThrowsArgumentNullException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BrandRepository(null!));
    }

    [Fact]
    public async Task TestGetAllAsync_HasRecords_ReturnsRecords()
    {
        // Arrange
        var context = DbContextHelper.CreateInMemoryDbContext();
        var expectedBrand = new Brand { Id = 1, Name = "Bronckhorst" };
        context.Brands.Add(expectedBrand);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        IRepository<Brand> brandRepository = new BrandRepository(context);

        // Act
        var result = await brandRepository.GetAllAsync();

        // Assert
        Assert.Equivalent(new List<Brand> { expectedBrand }, result);
    }

    [Fact]
    public async Task TestGetByIdAsync_HasRecord_ReturnsRecord()
    {
        // Arrange
        var context = DbContextHelper.CreateInMemoryDbContext();
        var expectedBrand = new Brand { Id = 1, Name = "Bronckhorst" };
        context.Brands.Add(expectedBrand);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        IRepository<Brand> brandRepository = new BrandRepository(context);

        // Act
        var result = await brandRepository.GetByIdAsync(expectedBrand.Id);

        // Assert
        Assert.Equivalent(expectedBrand, result);
    }

    [Fact]
    public async Task TestGetByIdAsync_UnknownId_ThrowsArgumentException()
    {
        // Arrange
        IRepository<Brand> brandRepository = new BrandRepository(DbContextHelper.CreateInMemoryDbContext());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => brandRepository.GetByIdAsync(999));
    }
}