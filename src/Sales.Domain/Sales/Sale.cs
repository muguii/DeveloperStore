using Sales.Domain.Abstractions;
using Sales.Domain.Products;
using Sales.Domain.Shared;

namespace Sales.Domain.Sales;

public sealed class Sale : BaseEntity
{
    public DateTime CreatedAt { get; private set; }
    public string Customer { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Branch { get; private set; }
    public List<SaleItem> Products { get; private set; } = [];
    public SaleStatus Status { get; private set; }

    private Sale()
    {
        
    }

    private Sale(string customer, string branch) : base(Guid.NewGuid())
    {
        Customer = customer;
        Branch = branch;
        CreatedAt = DateTime.UtcNow;

        Status = SaleStatus.NotCancelled;

        QueueDomainEvent(new SaleCreatedDomainEvent(Guid.NewGuid(), Id));
    }

    public static ValueResult<Sale> Create(string customer, string branch)
    {
        var errors = new List<FailureDetail>();

        if (string.IsNullOrWhiteSpace(customer))
            errors.Add(FailureDetail.NullValue(nameof(customer)));

        if (string.IsNullOrWhiteSpace(branch))
            errors.Add(FailureDetail.NullValue(nameof(branch)));

        return errors.Count == 0 ? ValueResult<Sale>.Success(new Sale(customer, branch))
                                 : ValueResult<Sale>.Failure(errors);
    }

    public void Update(string customer, string branch)
    {
        Customer = customer;
        Branch = branch;

        QueueDomainEvent(new SaleModifiedDomainEvent(Guid.NewGuid(), Id));
    }

    public ValueResult Cancel()
    {
        if (Status == SaleStatus.Cancelled)
            return ValueResult.Failure(SaleFailureDetail.SaleCancelledCannotBeChanged);

        Status = SaleStatus.Cancelled;

        QueueDomainEvent(new SaleCancelledDomainEvent(Guid.NewGuid(), Id));

        return ValueResult.Success();
    }

    public ValueResult<SaleItem> AddItem(Product product, int quantity)
    {
        if (Status == SaleStatus.Cancelled)
            return ValueResult<SaleItem>.Failure(SaleFailureDetail.SaleCancelledCannotBeChanged);

        var saleItemResult = SaleItem.Create(Id, product, quantity);
        if (saleItemResult.Succeeded)
        {
            Products.Add(saleItemResult.Value!);

            CalculateTotal();
        }

        return saleItemResult;
    }

    public ValueResult RemoveItemById(Guid itemId)
    {
        if (Status == SaleStatus.Cancelled)
            return ValueResult.Failure(SaleFailureDetail.SaleCancelledCannotBeChanged);

        var saleItem = Products.FirstOrDefault(si => si.Id == itemId);
        if (saleItem is null)
            return ValueResult.Failure(FailureDetail.ResourceNotFound(nameof(SaleItem), itemId.ToString()));

        Products.Remove(saleItem!);

        CalculateTotal();

        QueueDomainEvent(new SaleItemRemovedDomainEvent(Guid.NewGuid(), Id));

        return ValueResult.Success();
    }

    private void CalculateTotal()
    {
        TotalAmount = Products.Sum(si => si.TotalAmount);
    }
}