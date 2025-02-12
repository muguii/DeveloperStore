using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Sales.Queries.GetById;

public sealed record GetSaleByIdQuery : IRequest<ApplicationResult<SaleDto>>
{
    public Guid Id { get; init; }
}