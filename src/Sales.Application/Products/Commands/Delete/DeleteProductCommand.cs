using MediatR;
using Sales.Application.Shared;

namespace Sales.Application.Products.Commands.Delete;

public sealed record DeleteProductCommand : IRequest<ApplicationResult>
{
    public Guid Id { get; init; }
}