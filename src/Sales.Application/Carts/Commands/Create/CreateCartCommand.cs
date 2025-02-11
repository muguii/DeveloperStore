using MediatR;
using Sales.Application.Carts.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Carts.Commands.Create;

public sealed record CreateCartCommand : IRequest<ApplicationResult<CartDto>>
{
    public int UserId { get; init; }
    public DateTime Date { get; init; }
    public List<CreateCartItem> Products { get; init; }
}

public sealed record CreateCartItem
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}