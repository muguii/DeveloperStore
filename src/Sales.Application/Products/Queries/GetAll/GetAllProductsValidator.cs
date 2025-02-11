using FluentValidation;
using Sales.Application.Shared;

namespace Sales.Application.Products.Queries.GetAll;

public sealed class GetAllProductsValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsValidator()
    {
        RuleFor(g => g.MinPrice).GreaterThan(0)
                                .WithMessage("{PropertyName} must be greater than zero.")
                                .Unless(g => !g.MinPrice.HasValue);

        RuleFor(g => g.MaxPrice).GreaterThan(0)
                                .WithMessage("{PropertyName} must be greater than zero.")
                                .Unless(g => !g.MaxPrice.HasValue);

        RuleFor(g => g).SetValidator(new PagingFilterValidator());
    }
}