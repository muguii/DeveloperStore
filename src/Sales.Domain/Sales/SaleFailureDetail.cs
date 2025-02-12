using Sales.Domain.Shared;

namespace Sales.Domain.Sales;

internal static class SaleFailureDetail
{
    public static readonly FailureDetail SaleCancelledCannotBeChanged = new("InvalidOperation", "Invalid operation.", "The sale is cancelled and cannot be changed.");
}