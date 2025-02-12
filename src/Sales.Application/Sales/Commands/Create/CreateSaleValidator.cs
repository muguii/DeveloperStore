using FluentValidation;

namespace Sales.Application.Sales.Commands.Create;

public sealed class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.Customer).NotEmpty()
                                .WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Branch).NotEmpty()
                              .WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Products).NotEmpty()
                                .WithMessage("The sale must contain at least one product.")
                                .ForEach(x => x.SetValidator(new CreateSaleItemValidator()));
    }
}