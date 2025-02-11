using Sales.Domain.Abstractions;
using Sales.Domain.Products;
using Sales.Domain.Shared;

namespace Sales.Domain.Carts;

public sealed class CartItem : BaseEntity
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public int Quantity { get; private set; }

    private CartItem()
    {
        
    }

    private CartItem(Guid cartId, Product product, int quantity) : base(Guid.NewGuid())
    {
        CartId = cartId;
        Product = product;
        Quantity = quantity;
    }

    internal static ValueResult<CartItem> Create(Guid cartId, Product product, int quantity)
    {
        var errors = new List<FailureDetail>();

        if (cartId == Guid.Empty)
            errors.Add(FailureDetail.NullValue(nameof(cartId)));

        if (product is null)
            errors.Add(FailureDetail.NullValue(nameof(product)));

        if (quantity <= 0)
            errors.Add(FailureDetail.NegativeOrZeroValue(nameof(quantity)));

        return errors.Count == 0 ? ValueResult<CartItem>.Success(new CartItem(cartId, product!, quantity))
                                 : ValueResult<CartItem>.Failure(errors);
    }
}