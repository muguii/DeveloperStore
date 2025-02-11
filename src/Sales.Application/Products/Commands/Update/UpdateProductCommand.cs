using MediatR;
using Sales.Application.Products.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Products.Commands.Update;

public sealed record UpdateProductCommand : IRequest<ApplicationResult<ProductDto>>
{
    public Guid Id { get; set; }
    public string Title { get; init; }
    public decimal Price { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public string Image { get; init; }
    public UpdateProductRating Rating { get; init; }
}

public sealed record UpdateProductRating
{
    public decimal Rate { get; init; }
    public int Count { get; init; }
}