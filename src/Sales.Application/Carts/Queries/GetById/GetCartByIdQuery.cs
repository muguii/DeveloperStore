using MediatR;
using Sales.Application.Carts.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Carts.Queries.GetById;

public sealed record GetCartByIdQuery : IRequest<ApplicationResult<CartDto>>
{
    public Guid Id { get; init; }
}