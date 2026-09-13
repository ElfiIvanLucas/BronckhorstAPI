using BronckhorstWeb.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BronckhorstWeb.Pages;

public class OnzeProductenModel : PageModel
{
    private static readonly int[] SupportedPageSizes = [12, 24, 48];
    private readonly ProductApiClient _productApiClient;
    private readonly ILogger<OnzeProductenModel> _logger;

    public OnzeProductenModel(ProductApiClient productApiClient, ILogger<OnzeProductenModel> logger)
    {
        _productApiClient = productApiClient ?? throw new ArgumentNullException(nameof(productApiClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty(SupportsGet = true)]
    public int? BrandId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? CategoryId { get; set; }

    [BindProperty(SupportsGet = true)]
    public decimal? MinPrice { get; set; }

    [BindProperty(SupportsGet = true)]
    public decimal? MaxPrice { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Name { get; set; }

    [BindProperty(Name = "page", SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 12;

    [BindProperty(SupportsGet = true)]
    public ProductSort Sort { get; set; } = ProductSort.NameAsc;

    public IReadOnlyList<BrandDto> Brands { get; private set; } = [];

    public IReadOnlyList<CategoryDto> Categories { get; private set; } = [];

    public PageResult<ProductDto> Products { get; private set; } = new();

    public string? LoadError { get; private set; }

    public int TotalPages => Products.TotalCount == 0 ? 1 : (int)Math.Ceiling((double)Products.TotalCount / Products.PageSize);

    public bool HasPreviousPage => Products.Page > 1;

    public bool HasNextPage => Products.Page < TotalPages;

    public async Task OnGetAsync()
    {
        CurrentPage = Math.Max(CurrentPage, 1);
        PageSize = SupportedPageSizes.Contains(PageSize) ? PageSize : SupportedPageSizes[0];

        try
        {
            var brandsTask = _productApiClient.GetBrandsAsync();
            var categoriesTask = _productApiClient.GetCategoriesAsync();
            var productsTask = _productApiClient.GetProductsAsync(new ProductFilters
            {
                BrandId = BrandId,
                CategoryId = CategoryId,
                MinPrice = MinPrice,
                MaxPrice = MaxPrice,
                Name = Name,
                Page = CurrentPage,
                PageSize = PageSize,
                Sort = Sort
            });

            await Task.WhenAll(brandsTask, categoriesTask, productsTask);
            Brands = await brandsTask;
            Categories = await categoriesTask;
            Products = await productsTask;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Could not load the product catalog.");
            LoadError = "De productcatalogus kan momenteel niet worden geladen.";
        }
    }
}