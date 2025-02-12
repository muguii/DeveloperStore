namespace Sales.Domain.Sales.Discounts;

internal static class DiscountPercentageCalculator
{
    private static readonly TenPercentDiscountRuleHandler TenPercentHandler = new();
    private static readonly TwentyPercentDiscountRuleHandler TwentyPercentHandler = new();

    internal static decimal? Calculate(int quantity)
    {
        TenPercentHandler.SetNext(TwentyPercentHandler);

        return TenPercentHandler.Handle(quantity);
    }
}