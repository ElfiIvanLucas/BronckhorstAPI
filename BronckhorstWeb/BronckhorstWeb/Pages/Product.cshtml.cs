using BronckhorstWeb.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BronckhorstWeb.Pages;

public class ProductModel(ProductApiClient productApiClient, ILogger<ProductModel> logger) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ProductCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Dimension { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? ProductId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public IReadOnlyList<string> Dimensions { get; private set; } = [];
    public IReadOnlyList<ProductDto> Variants { get; private set; } = [];
    public ProductDto? SelectedProduct { get; private set; }
    public string? LoadError { get; private set; }

    public async Task OnGetAsync()
    {
        try
        {
            var products = await productApiClient.GetProductsByCodeAsync(ProductCode ?? string.Empty);
            Dimensions = products.Select(product => product.Dimensions)
                .Distinct(StringComparer.Ordinal)
                .OrderBy(dimension => dimension, StringComparer.Ordinal)
                .ToList();

            if (Dimensions.Count == 0)
            {
                return;
            }

            Dimension = Dimensions.FirstOrDefault(dimension => string.Equals(dimension, Dimension, StringComparison.Ordinal))
                        ?? Dimensions[0];
            Variants = products.Where(product => string.Equals(product.Dimensions, Dimension, StringComparison.Ordinal))
                .OrderBy(product => product.Id)
                .ToList();
            SelectedProduct = Variants.FirstOrDefault(product => product.Id == ProductId) ?? Variants[0];
            ProductId = SelectedProduct.Id;
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Could not load products for product code {ProductCode}.", ProductCode);
            LoadError = "De producten kunnen momenteel niet worden geladen.";
        }
    }

    public Dictionary<string, string> SelectionRouteValues(string dimension, int? productId = null)
    {
        var values = new Dictionary<string, string>
        {
            [nameof(ProductCode)] = ProductCode ?? string.Empty,
            [nameof(Dimension)] = dimension
        };

        if (productId.HasValue)
        {
            values[nameof(ProductId)] = productId.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        if (Url.IsLocalUrl(ReturnUrl))
        {
            values[nameof(ReturnUrl)] = ReturnUrl!;
        }

        return values;
    }
}
