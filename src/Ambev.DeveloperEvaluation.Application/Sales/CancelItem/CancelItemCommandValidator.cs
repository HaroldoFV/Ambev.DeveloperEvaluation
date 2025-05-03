using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Validator for <see cref="CancelItemCommand"/> that defines validation rules for cancel a sale item.
/// </summary>
public class CancelItemCommandValidator : AbstractValidator<CancelItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelItemCommandValidator"/> with defined validation rules.
    /// </summary>
    public CancelItemCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("SaleId is required");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("ItemId is required");
    }
}