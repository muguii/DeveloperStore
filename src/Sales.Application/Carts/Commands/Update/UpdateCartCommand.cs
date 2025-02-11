using MediatR;
using Sales.Application.Carts.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Carts.Commands.Update;

public sealed record UpdateCartCommand : IRequest<ApplicationResult<CartDto>>
{
    public Guid Id { get; set; }
    public int UserId { get; init; }
    public DateTime Date { get; init; }
    public List<UpdateCartItem> Products { get; init; }
}

public sealed record UpdateCartItem
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}