using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Domain.Entities;
using BronckhorstAPI.Infrastructure.Persistence.Repositories;
using BronckhorstAPI.Infrastructure.Tests.Helpers;

namespace BronckhorstAPI.Infrastructure.Tests.Repositories;

public class CategoryRepositoryTests
{
    [Fact]
    public void TestConstructor_NullArguments_ThrowsArgumentNullException()
    {
        // Arrange

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CategoryRepository(null!));
    }

    [Fact]
    public async Task TestGetAllAsync_HasRecords_ReturnsRecords()
    {
        // Arrange
        var context = DbContextHelper.CreateInMemoryDbContext();
        var expectedCategory = new Category { Id = 1, Name = "Category" };
        context.Categories.Add(expectedCategory);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        IRepository<Category> categoryRepository = new CategoryRepository(context);

        // Act
        var result = await categoryRepository.GetAllAsync();

        // Assert
        Assert.Equivalent(new List<Category> { expectedCategory }, result);
    }

    [Fact]
    public async Task TestGetByIdAsync_HasRecord_ReturnsRecord()
    {
        // Arrange
        var context = DbContextHelper.CreateInMemoryDbContext();
        var expectedCategory = new Category { Id = 1, Name = "Category" };
        context.Categories.Add(expectedCategory);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        IRepository<Category> categoryRepository = new CategoryRepository(context);

        // Act
        var result = await categoryRepository.GetByIdAsync(expectedCategory.Id);

        // Assert
        Assert.Equivalent(expectedCategory, result);
    }

    [Fact]
    public async Task TestGetByIdAsync_UnknownId_ThrowsArgumentException()
    {
        // Arrange
        IRepository<Category> categoryRepository = new CategoryRepository(DbContextHelper.CreateInMemoryDbContext());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => categoryRepository.GetByIdAsync(999));
    }
}