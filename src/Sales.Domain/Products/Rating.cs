using Sales.Domain.Shared;

namespace Sales.Domain.Products;

public sealed record Rating
{
    public decimal Rate { get; private set; }
    public int Count { get; private set; }

    private Rating()
    {
        
    }

    private Rating(decimal rate, int count)
    {
        Rate = rate;
        Count = count;
    }

    public static ValueResult<Rating> Create(decimal rate, int count)
    {
        var errors = new List<FailureDetail>();

        if (rate <= 0)
            errors.Add(FailureDetail.NegativeOrZeroValue(nameof(rate)));
        
        if (count <= 0)
            errors.Add(FailureDetail.NegativeOrZeroValue(nameof(count)));

        return errors.Count == 0 ? ValueResult<Rating>.Success(new Rating(rate, count))
                                 : ValueResult<Rating>.Failure(errors);
    }

    internal void Update(decimal rate, int count)
    {
        Rate = rate;
        Count = count;
    }
}