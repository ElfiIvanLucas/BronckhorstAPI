using System.Net;
using System.Net.Http.Json;
using BronckhorstWeb.Api;
using BronckhorstWeb.Pages;
using Microsoft.Extensions.Logging.Abstractions;

namespace BronckhorstWeb.Tests;

public class CatalogTests
{
    [Fact]
    public async Task Product_DefaultsToFirstStringDimensionAndFirstProduct_WithDuplicateDimensions()
    {
        var model = CreateProductModel();

        await model.OnGetAsync();

        Assert.Equal(["10", "2", "M"], model.Dimensions);
        Assert.Equal("10", model.Dimension);
        Assert.Equal([3, 4], model.Variants.Select(product => product.Id));
        Assert.Equal(3, model.SelectedProduct!.Id);
    }

    [Theory]
    [InlineData("2", 3, 1)]
    [InlineData("10", 4, 4)]
    [InlineData("invalid", 99, 3)]
    public async Task Product_SelectsOnlyProductsOfSelectedDimension_AndFallsBackForInvalidSelection(string dimension, int productId, int expectedId)
    {
        var model = CreateProductModel();
        model.Dimension = dimension;
        model.ProductId = productId;

        await model.OnGetAsync();

        Assert.Equal(expectedId, model.SelectedProduct!.Id);
        Assert.All(model.Variants, product => Assert.Equal(model.Dimension, product.Dimensions));
    }

    [Fact]
    public async Task Catalog_SendsBothCategoryIdsWithBrandSearchAndPaging()
    {
        ProductFilters? filters = null;
        using var client = CreateClient(async request =>
        {
            if (request.Method == HttpMethod.Post)
            {
                Assert.EndsWith("GetProductGroups", request.RequestUri!.AbsolutePath);
                filters = await request.Content!.ReadFromJsonAsync<ProductFilters>();
                return Json(new PageResult<ProductGroupDto> { Page = 2, PageSize = 24, TotalCount = 25 });
            }

            return request.RequestUri!.AbsolutePath.Contains("Category")
                ? Json(new[] { new CategoryDto { Id = 10 }, new CategoryDto { Id = 11, ParentId = 10 }, new CategoryDto { Id = 21, ParentId = 20 } })
                : Json(Array.Empty<BrandDto>());
        });
        var model = new OnzeProductenModel(new ProductApiClient(client), NullLogger<OnzeProductenModel>.Instance)
        {
            ParentCategoryId = 10, CategoryId = 11, BrandId = 7, Name = "ART", CurrentPage = 2, PageSize = 24, Sort = ProductSort.NameDesc
        };

        await model.OnGetAsync();

        Assert.NotNull(filters);
        Assert.Equal(10, filters.ParentCategoryId);
        Assert.Equal(11, filters.CategoryId);
        Assert.Equal(7, filters.BrandId);
        Assert.Equal("ART", filters.Name);
        Assert.Equal(2, filters.Page);
        Assert.Equal(24, filters.PageSize);
        Assert.Equal(ProductSort.NameDesc, filters.Sort);
        Assert.Equal(11, Assert.Single(model.GetSubCategories()).Id);
    }

    [Fact]
    public async Task Catalog_ClearsSubcategoryThatBelongsToAnotherParent()
    {
        ProductFilters? filters = null;
        using var client = CreateClient(async request =>
        {
            if (request.Method == HttpMethod.Post)
            {
                filters = await request.Content!.ReadFromJsonAsync<ProductFilters>();
                return Json(new PageResult<ProductGroupDto> { Page = 1, PageSize = 12 });
            }
            return request.RequestUri!.AbsolutePath.Contains("Category")
                ? Json(new[] { new CategoryDto { Id = 10 }, new CategoryDto { Id = 21, ParentId = 20 } })
                : Json(Array.Empty<BrandDto>());
        });
        var model = new OnzeProductenModel(new ProductApiClient(client), NullLogger<OnzeProductenModel>.Instance)
        {
            ParentCategoryId = 10, CategoryId = 21
        };

        await model.OnGetAsync();

        Assert.Equal(10, filters!.ParentCategoryId);
        Assert.Null(filters.CategoryId);
    }

    private static ProductModel CreateProductModel()
    {
        var client = CreateClient(request => Task.FromResult(Json(new[]
        {
            new ProductDto { Id = 4, ProductCode = "LAL", Dimensions = "10" },
            new ProductDto { Id = 1, ProductCode = "LAL", Dimensions = "2" },
            new ProductDto { Id = 3, ProductCode = "LAL", Dimensions = "10" },
            new ProductDto { Id = 5, ProductCode = "LAL", Dimensions = "M" }
        })));
        return new ProductModel(new ProductApiClient(client), NullLogger<ProductModel>.Instance) { ProductCode = "LAL" };
    }

    private static HttpResponseMessage Json<T>(T value) => new(HttpStatusCode.OK) { Content = JsonContent.Create(value) };

    private static HttpClient CreateClient(Func<HttpRequestMessage, Task<HttpResponseMessage>> response)
        => new(new StubHandler(response)) { BaseAddress = new Uri("http://catalog.test/") };

    private sealed class StubHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => response(request);
    }
}
