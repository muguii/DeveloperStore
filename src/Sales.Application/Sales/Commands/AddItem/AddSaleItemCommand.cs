using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Sales.Commands.AddItem;

public sealed record AddSaleItemCommand : IRequest<ApplicationResult<SaleDto>>
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}