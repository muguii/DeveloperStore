using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Sales.Commands.RemoveItem;

public sealed record RemoveSaleItemCommand : IRequest<ApplicationResult<SaleDto>>
{
    public Guid ItemId { get; init; }
    public Guid SaleId { get; set; }
}