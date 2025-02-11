using MediatR;
using Sales.Application.Shared;

namespace Sales.Application.Products.Commands.DeleteById;

public sealed record DeleteProductByIdCommand : IRequest<ApplicationResult>
{
    public Guid Id { get; init; }
}