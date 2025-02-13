using Sales.Domain.Shared;

namespace Sales.Domain.Sales;

internal static class SaleItemFailureDetail
{
    public static readonly FailureDetail MaximumQuantityExceeded = new("MaximumQuantityExceeded", "Maximum quantity exceeded.", $"A maximum of {SaleItem.MAX_QUANTITY_ALLOWED} items are allowed per product.");
}