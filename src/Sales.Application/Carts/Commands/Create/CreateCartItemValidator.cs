using FluentValidation;

namespace Sales.Application.Carts.Commands.Create;

public sealed class CreateCartItemValidator : AbstractValidator<CreateCartItem>
{
    public CreateCartItemValidator()
    {
        RuleFor(c => c.ProductId).NotEmpty()
                                 .WithMessage("{PropertyName} is required.");

        RuleFor(c => c.Quantity).GreaterThan(0)
                                .WithMessage("{PropertyName} must be greater than zero.");
    }
}