using MediatR;
using Sales.Application.Products.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Products.Queries.GetAll;

public sealed record GetAllProductsQuery : PagingFilter, IRequest<ApplicationResult<Paging<ProductDto>>>
{
    public List<string>? Title { get; init; }
    public List<string>? Description { get; init; }
    public List<string>? Category { get; init; }
    public List<string>? Images { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public string? OrderBy { get; init; }
}