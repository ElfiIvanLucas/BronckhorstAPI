namespace BronckhorstWeb.Api;

public class ProductApiClient
{
    private readonly HttpClient _httpClient;

    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId);

        return await _httpClient.GetFromJsonAsync<List<ProductDto>>(
                   $"api/v1/Product/GetProductsByCategory/{categoryId}")
               ?? [];
    }
}