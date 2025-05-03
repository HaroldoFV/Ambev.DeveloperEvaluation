namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Data transfer object for a sale item.
/// </summary>
/// <remarks>
/// Represents the details of an item in a sale, including product name, quantity, and price.
/// </remarks>
public class SaleItemDto
{
    /// <summary>
    /// Gets or sets the productId of the product.
    /// </summary>
    public Guid ProductId{ get; set; }
    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}