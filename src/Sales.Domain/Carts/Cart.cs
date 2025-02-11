using Sales.Domain.Abstractions;
using Sales.Domain.Products;
using Sales.Domain.Shared;

namespace Sales.Domain.Carts;

public sealed partial class Cart : BaseEntity
{
    public int UserId { get; private set; }
    public DateTime Date { get; private set; }
    public List<CartItem> Products { get; private set; } = [];

    private Cart()
    {

    }

    private Cart(int userId, DateTime date) : base(Guid.NewGuid())
    {
        UserId = userId;
        Date = date;
    }

    public static ValueResult<Cart> Create(int userId, DateTime date)
    {
        var errors = new List<FailureDetail>();

        if (userId <= 0)
            errors.Add(FailureDetail.NegativeOrZeroValue(nameof(userId)));

        return errors.Count == 0 ? ValueResult<Cart>.Success(new Cart(userId, date))
                                 : ValueResult<Cart>.Failure(errors);
    }

    public void Update(int userId, DateTime date)
    {
        UserId = userId;
        Date = date;
    }

    public ValueResult<CartItem> AddItem(Product product, int quantity)
    {
        var cartItemResult = CartItem.Create(this.Id, product, quantity);
        if (cartItemResult.Succeeded)
            this.Products.Add(cartItemResult.Value!);

        return cartItemResult;
    }

    public void RemoveAllItems()
    {
        Products.Clear();
    }
}