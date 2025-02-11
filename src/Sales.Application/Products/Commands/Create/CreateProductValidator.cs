﻿using FluentValidation;
using FluentValidation.Results;

namespace Sales.Application.Products.Commands.Create;

public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Title).NotEmpty()
                             .WithMessage("{PropertyName} is required.");

        RuleFor(p => p.Price).GreaterThan(0)
                             .WithMessage("{PropertyName} must be greater than zero.");

        RuleFor(p => p.Description).NotEmpty()
                                   .WithMessage("{PropertyName} is required.");

        RuleFor(p => p.Category).NotEmpty()
                                .WithMessage("{PropertyName} is required.");

        RuleFor(p => p.Image).NotEmpty()
                             .WithMessage("{PropertyName} is required.");

        RuleFor(p => p.Rating.Rate).GreaterThan(0)
                                   .WithMessage("{PropertyName} must be greater than zero.");
        
        RuleFor(p => p.Rating.Count).GreaterThan(0)
                                    .WithMessage("{PropertyName} must be greater than zero.");
    }

    protected override bool PreValidate(ValidationContext<CreateProductCommand> context, ValidationResult result)
    {
        if (context.InstanceToValidate is null)
            return false;

        if (context.InstanceToValidate.Rating is null)
            return false;

        return true;
    }
}