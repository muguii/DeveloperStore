namespace Sales.Domain.Sales.Discounts;

internal abstract class BaseDiscountPercentageRuleHandler : IDiscountPercentageRuleHandler
{
    private IDiscountPercentageRuleHandler nextHandler;

    public void SetNext(IDiscountPercentageRuleHandler handler)
    {
        nextHandler = handler;
    }

    public virtual decimal? Handle(int quantity)
    {
        return nextHandler?.Handle(quantity);
    }
}
