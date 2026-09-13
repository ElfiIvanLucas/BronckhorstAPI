using BronckhorstAPI.Application.Repositories.Interfaces;
using BronckhorstAPI.Application.Filters;
using BronckhorstAPI.Application.Pagination;
using BronckhorstAPI.Domain.Entities;
using BronckhorstAPI.Infrastructure.Persistence.Repositories;
using BronckhorstAPI.Infrastructure.Tests.Helpers;

namespace BronckhorstAPI.Infrastructure.Tests.Repositories;

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
    public async Task TestGetProductsByFiltersAsync_CategoryId_HasMatchingRecords_ReturnsMatchingRecords()
    {
        // Arrange
        const int categoryId = 1;
        var context = DbContextHelper.CreateInMemoryDbContext();
        var matchingProduct = new Product { Id = 3, Name = "Matching product" };
        var otherProduct = new Product { Id = 4, Name = "Other product" };
        context.Products.AddRange(matchingProduct, otherProduct);
        context.ProductCategories.AddRange(
            new ProductCategory { ProductId = matchingProduct.Id, CategoryId = categoryId },
            new ProductCategory { ProductId = otherProduct.Id, CategoryId = 2 });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var productRepository = new ProductRepository(context);

        // Act
        var result = await productRepository.GetProductsByFiltersAsync(new ProductFilters
        {
            CategoryId = categoryId,
            Page = 1,
            PageSize = 10
        });

        // Assert
        var actualProduct = Assert.Single(result.Items);
        Assert.Equal(matchingProduct.Id, actualProduct.Id);
    }

    [Theory]
    [MemberData(nameof(ProductFilterData))]
    public async Task TestGetProductsByFiltersAsync_FilterValues_ReturnsMatchingProducts(ProductFilters productFilters,
        int expectedProductId)
    {
        // Arrange
        var context = DbContextHelper.CreateInMemoryDbContext();
        context.Products.AddRange(
            new Product { Id = 3, Name = "Desk", BrandId = 1, Price = 50 },
            new Product { Id = 4, Name = "Chair", BrandId = 2, Price = 150 });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var productRepository = new ProductRepository(context);

        // Act
        var result = await productRepository.GetProductsByFiltersAsync(productFilters);

        // Assert
        var actualProduct = Assert.Single(result.Items);
        Assert.Equal(expectedProductId, actualProduct.Id);
    }

    [Theory]
    [MemberData(nameof(ProductSortData))]
    public async Task TestGetProductsByFiltersAsync_SortValues_ReturnsSortedProducts(ProductSort sort,
        int expectedFirstProductId)
    {
        // Arrange
        var productRepository = new ProductRepository(DbContextHelper.CreateInMemoryDbContext());
        var productFilters = new ProductFilters { Page = 1, PageSize = 10, Sort = sort };

        // Act
        var result = await productRepository.GetProductsByFiltersAsync(productFilters);

        // Assert
        Assert.Equal(expectedFirstProductId, result.Items.First().Id);
    }

    [Fact]
    public async Task TestGetProductsByFiltersAsync_Pagination_ReturnsRequestedPage()
    {
        // Arrange
        var productRepository = new ProductRepository(DbContextHelper.CreateInMemoryDbContext());
        var productFilters = new ProductFilters { Page = 2, PageSize = 1 };

        // Act
        var result = await productRepository.GetProductsByFiltersAsync(productFilters);

        // Assert
        var actualProduct = Assert.Single(result.Items);
        Assert.Equal(2, actualProduct.Id);
        Assert.Equal(2, result.TotalCount);
    }

    [Fact]
    public async Task TestGetProductsByFiltersAsync_UnknownSort_ReturnsUnsortedProducts()
    {
        // Arrange
        var productRepository = new ProductRepository(DbContextHelper.CreateInMemoryDbContext());
        var productFilters = new ProductFilters { Page = 1, PageSize = 10, Sort = (ProductSort)999 };

        // Act
        var result = await productRepository.GetProductsByFiltersAsync(productFilters);

        // Assert
        Assert.Equivalent(DbContextHelper.CreateProductEntities(), result.Items);
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

    public static TheoryData<ProductFilters, int> ProductFilterData =>
    [
        (new ProductFilters { BrandId = 1, Page = 1, PageSize = 10 }, 3),
        (new ProductFilters { MinPrice = 50, MaxPrice = 50, Page = 1, PageSize = 10 }, 3),
        (new ProductFilters { Name = "Desk", Page = 1, PageSize = 10 }, 3)
    ];

    public static TheoryData<ProductSort, int> ProductSortData =>
    [
        (ProductSort.NameAsc, 1),
        (ProductSort.NameDesc, 2),
        (ProductSort.PriceAsc, 1),
        (ProductSort.PriceDesc, 2)
    ];
}