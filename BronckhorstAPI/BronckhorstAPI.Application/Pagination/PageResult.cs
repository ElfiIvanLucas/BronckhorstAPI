namespace BronckhorstAPI.Application.Pagination;

public class PageResult<T>
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public IEnumerable<T> Items { get; init; } = [];
}
