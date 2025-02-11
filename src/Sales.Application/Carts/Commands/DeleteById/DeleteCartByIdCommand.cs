using MediatR;
using Sales.Application.Shared;

namespace Sales.Application.Carts.Commands.DeleteById;

public sealed record DeleteCartByIdCommand : IRequest<ApplicationResult>
{
    public Guid Id { get; init; }
}