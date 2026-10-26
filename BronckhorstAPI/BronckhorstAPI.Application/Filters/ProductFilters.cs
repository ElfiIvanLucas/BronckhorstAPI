using BronckhorstAPI.Application.Pagination;

namespace BronckhorstAPI.Application.Filters;

public class ProductFilters
{
    public int? BrandId { get; init; }
    public int? ParentCategoryId { get; init; }
    public int? CategoryId { get; init; }
    public string? Name { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;
    public ProductSort Sort { get; init; } = ProductSort.NameAsc;
}
