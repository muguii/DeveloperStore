using FluentValidation;

namespace Sales.Application.Sales.Commands.Create;

public sealed class CreateSaleItemValidator : AbstractValidator<CreateSaleItem>
{
    public CreateSaleItemValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty()
                                 .WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Quantity).GreaterThan(0)
                                .WithMessage("{PropertyName} must be greater than zero.");
    }
}