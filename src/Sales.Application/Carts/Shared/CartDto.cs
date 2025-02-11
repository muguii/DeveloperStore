namespace Sales.Application.Carts.Shared;

public sealed record CartDto
{
    public Guid Id { get; init; }
    public int UserId { get; init; }
    public DateTime Date { get; init; }
    public List<CartItemDto> Products { get; init; }
}