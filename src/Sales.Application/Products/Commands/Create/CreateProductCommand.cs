using MediatR;
using Sales.Application.Products.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Products.Commands.Create;

public sealed record CreateProductCommand : IRequest<ApplicationResult<ProductDto>>
{
    public string Title { get; init; }
    public decimal Price { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public string Image { get; init; }
    public CreteProductRating Rating { get; init; }
}

public sealed record CreteProductRating
{
    public decimal Rate { get; init; }
    public int Count { get; init; }
}