namespace Sales.Domain.Sales.Discounts;

internal class TwentyPercentDiscountRuleHandler : BaseDiscountPercentageRuleHandler
{
    public override decimal? Handle(int quantity)
    {
        if (quantity >= 10 && quantity <= 20)
            return 0.2M;

        return base.Handle(quantity);
    }
}
