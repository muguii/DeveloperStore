using FluentValidation;

namespace Sales.Application.Sales.Commands.AddItem;

public sealed class AddSaleItemValidator : AbstractValidator<AddSaleItemCommand>
{
    public AddSaleItemValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty()
                                 .WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Quantity).GreaterThan(0)
                                .WithMessage("{PropertyName} must be greater than zero.");
    }
}