namespace BronckhorstWeb.Api;

public class ProductFilters
{
    public int? BrandId { get; init; }

    public int? CategoryId { get; init; }

    public decimal? MinPrice { get; init; }

    public decimal? MaxPrice { get; init; }

    public string? Name { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 12;

    public ProductSort Sort { get; init; } = ProductSort.NameAsc;
}

public enum ProductSort
{
    NameAsc,
    NameDesc,
    PriceAsc,
    PriceDesc
}