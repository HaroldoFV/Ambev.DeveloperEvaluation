using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for <see cref="UpdateSaleCommand"/> that defines validation rules for updating a sale.
/// </summary>
public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleCommandValidator"/> with defined validation rules.
    /// </summary>
    public UpdateSaleCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("SaleId is required");

        RuleFor(command => command.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("BranchId is required");

        RuleFor(command => command.Items)
            .NotEmpty().WithMessage("At least one sale item is required.");

        RuleForEach(command => command.Items).ChildRules(items =>
        {
            items.RuleFor(item => item.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            items.RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            items.RuleFor(item => item.UnitPrice)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        });
    }
}