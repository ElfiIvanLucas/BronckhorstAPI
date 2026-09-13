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
        return await GetAsync<IReadOnlyList<BrandDto>>("api/v1/Brand/GetProducts");
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
    {
        return await GetAsync<IReadOnlyList<CategoryDto>>("api/v1/Category/GetCategories");
    }

    public async Task<PageResult<ProductDto>> GetProductsAsync(ProductFilters productFilters)
    {
        ArgumentNullException.ThrowIfNull(productFilters);

        using var response = await _httpClient.PostAsJsonAsync("api/v1/Product/GetProductsByFilters", productFilters);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PageResult<ProductDto>>()
               ?? throw new InvalidOperationException("The product API returned an empty response.");
    }

    private async Task<T> GetAsync<T>(string requestUri)
    {
        using var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>()
               ?? throw new InvalidOperationException("The API returned an empty response.");
    }
}