using FluentValidation;
using Sales.Application.Shared;

namespace Sales.Application.Products.Queries.GetByCategory;

public sealed class GetProductsByCategoryValidator : AbstractValidator<GetProductByCategoryQuery>
{
    public GetProductsByCategoryValidator()
    {
        RuleFor(g => g).SetValidator(new PagingFilterValidator());
    }
}