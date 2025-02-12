using FluentValidation;

namespace Sales.Application.Sales.Commands.RemoveItem;

public sealed class RemoveSaleItemValidator : AbstractValidator<RemoveSaleItemCommand>
{
    public RemoveSaleItemValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty()
                          .WithMessage("{PropertyName} is required.");
    }
}