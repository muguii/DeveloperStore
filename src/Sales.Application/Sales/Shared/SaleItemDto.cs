namespace Sales.Application.Sales.Shared;

public sealed record SaleItemDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal Discount { get; init; }
    public decimal TotalAmount { get; init; }
}