using FluentValidation;
using Sales.Application.Shared;

namespace Sales.Application.Carts.Queries.GetAll;

public sealed class GetAllCartsValidator : AbstractValidator<GetAllCartsQuery>
{
    public GetAllCartsValidator()
    {
        RuleFor(g => g.MinDate).LessThanOrEqualTo(g => g.MaxDate)
                               .When(g => g.MinDate.HasValue && g.MaxDate.HasValue)
                               .WithMessage("{PropertyName} cannot be greater than {ComparisonProperty}.");

        RuleFor(g => g.MaxDate).GreaterThanOrEqualTo(g => g.MinDate)
                               .When(g => g.MinDate.HasValue && g.MaxDate.HasValue)
                               .WithMessage("{PropertyName} cannot be less than {ComparisonProperty}.");

        RuleFor(g => g).SetValidator(new PagingFilterValidator());
    }
}