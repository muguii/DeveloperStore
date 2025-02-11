namespace Sales.Application.Shared;

public sealed record Paging<T> where T : class
{
    public List<T> Data { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public Paging(List<T>? data, int currentPage, int pageSize, int totalItems)
    {
        Data = data ?? [];
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}