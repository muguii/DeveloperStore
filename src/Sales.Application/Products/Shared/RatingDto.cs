namespace Sales.Application.Products.Shared;

public sealed record RatingDto
{
    public decimal Rate { get; init; }
    public int Count { get; init; }
}