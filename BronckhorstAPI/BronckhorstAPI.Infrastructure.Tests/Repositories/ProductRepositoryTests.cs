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
        (new ProductFilters { Name = "Desk", Page = 1, PageSize = 10 }, 3)
    ];

    public static TheoryData<ProductSort, int> ProductSortData =>
    [
        (ProductSort.NameAsc, 1),
        (ProductSort.NameDesc, 2),
        (ProductSort.PriceAsc, 1),
        (ProductSort.PriceDesc, 2)
    ];

    [Fact]
    public async Task GetProductGroups_GroupsBeforePaging_AndSearchesByProductCode()
    {
        using var context = DbContextHelper.CreateInMemoryDbContext();
        context.Products.AddRange(
            new Product { Id = 3, ProductCode = "LAL", Name = "Z", Price = 30, Image = "lal" },
            new Product { Id = 4, ProductCode = "LAL", Name = "A", Price = 20 },
            new Product { Id = 5, ProductCode = "LAX", Name = "B", Price = 10 });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var repository = new ProductRepository(context);

        var firstPage = await repository.GetProductGroupsAsync(new ProductFilters { Name = "LA", PageSize = 1 });
        var secondPage = await repository.GetProductGroupsAsync(new ProductFilters { Name = "LA", PageSize = 1, Page = 2 });

        var group = Assert.Single(firstPage.Items);
        Assert.Equal("LAL", group.ProductCode);
        Assert.Equal(2, group.ProductCount);
        Assert.Equal(20, group.FromPrice);
        Assert.Equal("lal", group.Image);
        Assert.Equal(2, firstPage.TotalCount);
        Assert.Equal("LAX", Assert.Single(secondPage.Items).ProductCode);
    }

    [Theory]
    [InlineData(ProductSort.NameAsc, "LAL")]
    [InlineData(ProductSort.NameDesc, "LAX")]
    [InlineData(ProductSort.PriceAsc, "LAX")]
    [InlineData(ProductSort.PriceDesc, "LAL")]
    public async Task GetProductGroups_SortsByCodeOrStartingPrice(ProductSort sort, string expectedCode)
    {
        using var context = DbContextHelper.CreateInMemoryDbContext();
        context.Products.AddRange(
            new Product { Id = 3, ProductCode = "LAL", Name = "Z", Price = 30 },
            new Product { Id = 4, ProductCode = "LAX", Name = "A", Price = 10 });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await new ProductRepository(context).GetProductGroupsAsync(new ProductFilters { Name = "LA", Sort = sort });

        Assert.Equal(expectedCode, result.Items.First().ProductCode);
    }

    [Theory]
    [InlineData(10, null, null, 2)]
    [InlineData(10, 11, null, 1)]
    [InlineData(20, 11, null, 0)]
    [InlineData(10, null, 2, 1)]
    public async Task GetProductGroups_CombinesParentChildBrandAndSearchFilters(int parentId, int? categoryId, int? brandId, int expectedCount)
    {
        using var context = DbContextHelper.CreateInMemoryDbContext();
        context.Categories.AddRange(
            new Category { Id = 10 }, new Category { Id = 20 },
            new Category { Id = 11, ParentId = 10 }, new Category { Id = 12, ParentId = 10 },
            new Category { Id = 21, ParentId = 20 });
        context.Products.AddRange(
            new Product { Id = 3, ProductCode = "LAL", ArtikelCode = "FIND-1", BrandId = 1 },
            new Product { Id = 4, ProductCode = "LAX", ArtikelCode = "FIND-2", BrandId = 2 },
            new Product { Id = 5, ProductCode = "OTHER", BrandId = 1 });
        context.ProductCategories.AddRange(
            new ProductCategory { ProductId = 3, CategoryId = 11 },
            new ProductCategory { ProductId = 4, CategoryId = 12 },
            new ProductCategory { ProductId = 5, CategoryId = 21 });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await new ProductRepository(context).GetProductGroupsAsync(new ProductFilters
        {
            ParentCategoryId = parentId, CategoryId = categoryId, BrandId = brandId, Name = "FIND"
        });

        Assert.Equal(expectedCount, result.TotalCount);
    }

    [Fact]
    public async Task GetProductsByCode_ReturnsEveryVariantInIdOrder_AndMatchesExactCode()
    {
        using var context = DbContextHelper.CreateInMemoryDbContext();
        context.Products.AddRange(Enumerable.Range(3, 20).Select(id => new Product
        {
            Id = id, ProductCode = "LAL", Dimensions = id % 2 == 0 ? "10" : "2"
        }));
        context.Products.Add(new Product { Id = 23, ProductCode = "LAL-OTHER" });
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await new ProductRepository(context).GetProductsByCodeAsync("LAL");

        Assert.Equal(Enumerable.Range(3, 20), result.Select(product => product.Id));
        Assert.All(result, product => Assert.Equal("LAL", product.ProductCode));
    }
}
