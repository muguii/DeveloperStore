using FluentValidation;

namespace Sales.Application.Sales.Commands.Update;

public sealed class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleValidator()
    {
        RuleFor(x => x.Customer).NotEmpty()
                                .WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Branch).NotEmpty()
                              .WithMessage("{PropertyName} is required.");
    }
}