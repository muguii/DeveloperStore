namespace Sales.Domain.Sales.Discounts;

internal class TenPercentDiscountRuleHandler : BaseDiscountPercentageRuleHandler
{
    public override decimal? Handle(int quantity)
    {
        if (quantity >= 4 && quantity < 10)
            return 0.1M;

        return base.Handle(quantity);
    }
}
