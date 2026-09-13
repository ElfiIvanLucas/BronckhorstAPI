using BronckhorstAPI.Application.Pagination;

namespace BronckhorstAPI.Application.Filters;

public class ProductFilters
{
    public int? BrandId { get; init; }
    public int? CategoryId { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public string? Name { get; init; }

    public int Page { get; init; }
    public int PageSize { get; init; }
    public ProductSort Sort { get; init; }
}
