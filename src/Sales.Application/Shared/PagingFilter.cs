namespace Sales.Application.Shared;

public record PagingFilter
{
    private const int DEFAULT_PAGE = 1;
    private const int DEFAULT_SIZE = 10;

    private int _page = DEFAULT_PAGE;
    private int _size = DEFAULT_SIZE;

    public int Page
    {
        get => _page;
        init => _page = value > 0 ? value : DEFAULT_PAGE;
    }

    public int Size
    {
        get => _size;
        init => _size = value > 0 ? value : DEFAULT_SIZE;
    }

    public int Skip => (Page - 1) * Size;
}