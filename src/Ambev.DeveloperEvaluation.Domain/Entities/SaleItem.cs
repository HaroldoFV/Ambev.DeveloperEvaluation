using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale transaction.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Identifier of the product being sold.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Quantity of the product being sold.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Discount applied to the product.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Total value of the sale item after applying the discount.
    /// </summary>
    public decimal TotalValue { get; private set; }

    /// <summary>
    /// Identifier of the sale to which this item belongs.
    /// </summary>
    public Guid SaleId { get; private set; }

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    /// <param name="productId">Product identifier.</param>
    /// <param name="quantity">Quantity of the product.</param>
    /// <param name="unitPrice">Unit price of the product.</param>
    public SaleItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        ApplyDiscount();
        CalculateTotal();
    }

    /// <summary>
    /// Associates the sale item with a specific sale.
    /// </summary>
    /// <param name="saleId">Sale identifier.</param>
    public void AssociateItem(Guid saleId)
    {
        if (saleId == Guid.Empty)
            throw new ArgumentException("SaleId cannot be empty.");

        SaleId = saleId;
    }

    /// <summary>
    /// Adds units to the sale item.
    /// </summary>
    /// <param name="quantity">Number of units to add.</param>
    public void AddUnits(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        Quantity += quantity;
        ApplyDiscount();
        CalculateTotal();
    }

    /// <summary>
    /// Applies a discount based on the quantity of the product.
    /// </summary>
    private void ApplyDiscount()
    {
        Discount = Quantity switch
        {
            >= 10 and <= 20 => 0.20m,
            >= 4 and < 10 => 0.10m,
            _ => 0.0m
        };
    }

    /// <summary>
    /// Calculates the total value of the sale item.
    /// </summary>
    private void CalculateTotal() =>
        TotalValue = Quantity * UnitPrice * (1 - Discount);
}