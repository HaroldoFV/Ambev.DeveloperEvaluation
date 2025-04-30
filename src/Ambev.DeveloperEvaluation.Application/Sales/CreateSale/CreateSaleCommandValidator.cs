using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for <see cref="CreateSaleCommand"/> that defines validation rules for creating a sale.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleCommandValidator"/> with defined validation rules.
    /// </summary>
    public CreateSaleCommandValidator()
    {
        RuleFor(command => command.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(command => command.Items)
            .NotEmpty().WithMessage("At least one sale item is required.");

        RuleForEach(command => command.Items).ChildRules(items =>
        {
            items.RuleFor(item => item.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            items.RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            items.RuleFor(item => item.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        });
    }
}