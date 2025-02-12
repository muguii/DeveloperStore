using MediatR;
using Sales.Application.Sales.Shared;
using Sales.Application.Shared;

namespace Sales.Application.Sales.Commands.Create;

public sealed record CreateSaleCommand : IRequest<ApplicationResult<SaleDto>>
{
    public string Customer { get; init; }
    public string Branch { get; init; }
    public List<CreateSaleItem> Products { get; init; }
}

public sealed record CreateSaleItem
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}