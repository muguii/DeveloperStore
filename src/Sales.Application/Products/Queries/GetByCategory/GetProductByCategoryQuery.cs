using MediatR;
using Sales.Application.Products.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Products.Queries.GetByCategory;

public sealed record GetProductByCategoryQuery : PagingFilter, IRequest<ApplicationResult<Paging<ProductDto>>>
{
    public string Category { get; set; }
    public string? OrderBy { get; init; }
}