using Sales.Domain.Sales;

namespace Sales.Application.Sales.Shared;

public sealed record SaleDto
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Customer { get; init; }
    public decimal TotalAmount { get; init; }
    public string Branch { get; init; }
    public List<SaleItemDto> Products { get; init; }
    public SaleStatus Status { get; init; }
}