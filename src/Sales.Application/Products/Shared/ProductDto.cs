namespace Sales.Application.Products.Shared;

public sealed record ProductDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public decimal Price { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public string Image { get; init; }
    public RatingDto Rating { get; init; }
}