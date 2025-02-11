using FluentValidation;

namespace Sales.Application.Carts.Commands.Update;

public sealed class UpdateCartItemValidator : AbstractValidator<UpdateCartItem>
{
    public UpdateCartItemValidator()
    {
        RuleFor(c => c.ProductId).NotEmpty()
                                 .WithMessage("{PropertyName} is required.");

        RuleFor(c => c.Quantity).GreaterThan(0)
                                .WithMessage("{PropertyName} must be greater than zero.");
    }
}