using FluentValidation;

namespace Sales.Application.Carts.Commands.Update;

public sealed class UpdateCartValidator : AbstractValidator<UpdateCartCommand>
{
    public UpdateCartValidator()
    {
        RuleFor(c => c.UserId).GreaterThan(0)
                              .WithMessage("{PropertyName} must be greater than zero.");

        RuleFor(c => c.Date).NotEmpty()
                            .WithMessage("{PropertyName} is required.");

        RuleFor(c => c.Products).NotEmpty()
                                .WithMessage("The cart must contain at least one product.")
                                .ForEach(p => p.SetValidator(new UpdateCartItemValidator()));
    }
}