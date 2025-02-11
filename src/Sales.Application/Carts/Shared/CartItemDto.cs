namespace Sales.Application.Carts.Shared;

public sealed record CartItemDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}