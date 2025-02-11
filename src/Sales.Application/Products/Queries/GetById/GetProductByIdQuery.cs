using MediatR;
using Sales.Application.Products.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Products.Queries.GetById;

public sealed record GetProductByIdQuery : IRequest<ApplicationResult<ProductDto>>
{
    public Guid Id { get; init; }
}