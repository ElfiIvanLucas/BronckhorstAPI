namespace BronckhorstWeb.Api;

public class ProductApiClient
{
    private readonly HttpClient _httpClient;

    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IReadOnlyList<BrandDto>> GetBrandsAsync()
    {
        return await GetCollectionAsync<BrandDto>("api/v1/Brand/GetProducts");
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
    {
        return await GetCollectionAsync<CategoryDto>("api/v1/Category/GetCategories");
    }

    public async Task<PageResult<ProductDto>> GetProductsAsync(ProductFilters productFilters)
    {
        ArgumentNullException.ThrowIfNull(productFilters);

        using var response = await _httpClient.PostAsJsonAsync("api/v1/Product/GetProductsByFilters", productFilters);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PageResult<ProductDto>>()
               ?? throw new InvalidOperationException("The product API returned an empty response.");
    }

    private async Task<IReadOnlyList<T>> GetCollectionAsync<T>(string requestUri)
    {
        using var response = await _httpClient.GetAsync(requestUri);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return [];
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IReadOnlyList<T>>()
               ?? throw new InvalidOperationException("The API returned an empty response.");
    }
}