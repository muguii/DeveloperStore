using FluentValidation;

namespace Sales.Application.Shared;

public sealed class PagingFilterValidator : AbstractValidator<PagingFilter>
{
    public PagingFilterValidator()
    {
        RuleFor(p => p.Page).GreaterThan(0)
                            .WithMessage("{PropertyName} must be greater than {ComparisonValue}.");

        RuleFor(p => p.Size).GreaterThan(0)
                            .WithMessage("{PropertyName} must be greater than {ComparisonValue}.");
    }
}