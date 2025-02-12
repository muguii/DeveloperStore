namespace Sales.Domain.Sales.Discounts;

internal interface IDiscountPercentageRuleHandler
{
    void SetNext(IDiscountPercentageRuleHandler handler);
    decimal? Handle(int quantity);
}
