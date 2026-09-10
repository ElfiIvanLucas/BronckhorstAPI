using Domain.Entities;
using Persistence.Repositories;
using Persistence.Repositories.Interfaces;
using Persistence.Tests.Helpers;

namespace Persistence.Tests.Repositories;

public class ProductRepositoryTests
{
    private readonly IProductRepository _productRepository;

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
    public async Task TestGetByCategoryIdAsync_HasMatchingRecords_ReturnsMatchingRecords()
    {
        // Arrange
        const int categoryId = 1;
        var context = DbContextHelper.CreateInMemoryDbContext();
        var matchingProduct = new Product { Id = 1, Name = "Matching product" };
        var otherProduct = new Product { Id = 2, Name = "Other product" };
        context.Products.AddRange(matchingProduct, otherProduct);
        context.ProductCategories.AddRange(
            new ProductCategory { ProductId = matchingProduct.Id, CategoryId = categoryId },
            new ProductCategory { ProductId = otherProduct.Id, CategoryId = 2 });
        await context.SaveChangesAsync();
        var productRepository = new ProductRepository(context);

        // Act
        var result = await productRepository.GetByCategoryIdAsync(categoryId);

        // Assert
        var actualProduct = Assert.Single(result);
        Assert.Equal(matchingProduct.Id, actualProduct.Id);
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
