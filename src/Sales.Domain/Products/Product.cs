using Sales.Domain.Abstractions;
using Sales.Domain.Shared;

namespace Sales.Domain.Products;

public sealed class Product : BaseEntity
{
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public string Image { get; private set; }
    public Rating Rating { get; private set; }

    private Product()
    {
        
    }

    private Product(string title, decimal price, string description, string category, string image, Rating rating) : base(Guid.NewGuid())
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
    }

    public static ValueResult<Product> Create(string title, decimal price, string description, string category, string image, Rating rating)
    {
        var errors = new List<FailureDetail>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(FailureDetail.NullValue(nameof(title)));

        if (price <= 0)
            errors.Add(FailureDetail.NegativeOrZeroValue(nameof(price)));

        if (string.IsNullOrWhiteSpace(description))
            errors.Add(FailureDetail.NullValue(nameof(description)));

        if (string.IsNullOrWhiteSpace(category))
            errors.Add(FailureDetail.NullValue(nameof(category)));

        if (string.IsNullOrWhiteSpace(image))
            errors.Add(FailureDetail.NullValue(nameof(image)));

        if (rating is null)
            errors.Add(FailureDetail.NullValue(nameof(rating)));

        return errors.Count == 0 ? ValueResult<Product>.Success(new Product(title, price, description, category, image, rating!))
                                 : ValueResult<Product>.Failure(errors);
    }

    public void Update(string title, decimal price, string description, string category, string image, decimal rate, int count)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;

        Rating.Update(rate, count);
    }
}