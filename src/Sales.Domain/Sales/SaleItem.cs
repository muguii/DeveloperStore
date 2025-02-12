using Sales.Domain.Abstractions;
using Sales.Domain.Products;
using Sales.Domain.Sales.Discounts;
using Sales.Domain.Shared;

namespace Sales.Domain.Sales;

public sealed class SaleItem : BaseEntity
{
    private const byte MAX_QUANTITY_ALLOWED = 20;

    public Guid SaleId { get; private set; }
    public Product Product { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }

    private SaleItem()
    {

    }

    private SaleItem(Guid saleId, Product product, int quantity) : base(Guid.NewGuid())
    {
        SaleId = saleId;
        Product = product;
        Quantity = quantity;
        UnitPrice = product.Price;

        //// Simple approach
        // Discount = CalculateDiscount();

        var discountPercentage = DiscountPercentageCalculator.Calculate(Quantity);
        if (discountPercentage.HasValue)
            Discount = UnitPrice * Quantity * discountPercentage.Value;

        TotalAmount = (UnitPrice * Quantity) - Discount;
    }

    public static ValueResult<SaleItem> Create(Guid saleId, Product product, int quantity)
    {
        var errors = new List<FailureDetail>();

        if (saleId == Guid.Empty)
            errors.Add(FailureDetail.NullValue(nameof(saleId)));

        if (product is null)
            errors.Add(FailureDetail.NullValue(nameof(product)));

        if (quantity <= 0)
            errors.Add(FailureDetail.NegativeOrZeroValue(nameof(quantity)));

        if (quantity > MAX_QUANTITY_ALLOWED)
            errors.Add(new FailureDetail("MaximumQuantityExceeded", "Maximum quantity exceeded.", $"A maximum of {MAX_QUANTITY_ALLOWED} items are allowed per product."));

        if (product?.Price <= 0)
            errors.Add(FailureDetail.NegativeOrZeroValue(nameof(product.Price)));

        return errors.Count == 0 ? ValueResult<SaleItem>.Success(new SaleItem(saleId, product!, quantity))
                                 : ValueResult<SaleItem>.Failure(errors);
    }

    //// Simple approach
    //private decimal CalculateDiscount()
    //{
    //    var total = Quantity * UnitPrice;

    //    if (Quantity >= 10)
    //        return total * 0.2M;

    //    if (Quantity >= 4)
    //        return total * 0.1M;

    //    return 0M;
    //}
}