using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Sales.Commands.Update;

public sealed record UpdateSaleCommand : IRequest<ApplicationResult<SaleDto>>
{
    public Guid Id { get; set; }
    public string Customer { get; init; }
    public string Branch { get; init; }
}