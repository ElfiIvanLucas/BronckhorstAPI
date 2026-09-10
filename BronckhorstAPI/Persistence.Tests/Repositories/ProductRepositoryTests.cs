using Domain.Entities;
using Persistence.Repositories;
using Persistence.Repositories.Interfaces;
using Persistence.Tests.Helpers;

namespace Persistence.Tests.Repositories;

public class ProductRepositoryTests
{
    private readonly IRepository<Product> _productRepository;

    public ProductRepositoryTests()
    {
        var context = DbContextHelper.CreateInMemoryDbContext();
        _productRepository = new ProductRepository(context);
    }

    [Fact]
    public void TestConstructor_NullArguments_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ProductRepository(null!));
    }

    [Fact]
    public async Task TestGetAllAsync_HasRecords_ReturnsRecords()
    {
        // Arrange
        var expectedResult = DbContextHelper.CreateProductEntities();

        // Act
        var result = await _productRepository.GetAllAsync();

        // Assert
        Assert.Equivalent(expectedResult, result);
    }

    [Fact]
    public async Task TestGetByIdAsync_HasRecords_ReturnsRecord()
    {
        // Arrange
        const int id = 1;
        var expectedResult = DbContextHelper.CreateProductEntities().FirstOrDefault(p => p.Id == id);

        // Act
        var result = await _productRepository.GetByIdAsync(id);

        // Assert
        Assert.Equivalent(expectedResult, result);
    }

    [Fact]
    public async Task TestGetByIdAsync_UnknownId_ThrowsArgumentException()
    {
        // Arrange
        const int id = 999;

        // Act && Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productRepository.GetByIdAsync(id));
    }
}