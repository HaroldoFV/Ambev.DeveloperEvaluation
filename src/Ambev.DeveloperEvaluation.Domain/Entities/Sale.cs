using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// Unique sale number for identification.
    /// </summary>
    public long SaleNumber { get; private set; }

    /// <summary>
    /// Date and time when the sale was created.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Identifier of the customer associated with the sale.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Identifier of the branch where the sale occurred.
    /// </summary>
    public Guid BranchId { get; private set; }

    private readonly List<SaleItem> _items = new();

    /// <summary>
    /// List of items included in the sale.
    /// </summary>
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Total value of the sale.
    /// </summary>
    public decimal TotalValue { get; private set; }

    /// <summary>
    /// Current status of the sale.
    /// </summary>
    public SaleStatus Status { get; private set; }

    /// <summary>
    /// Date and time when the sale was created in the system.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    /// <param name="customerId">Customer identifier.</param>
    /// <param name="branchId">Branch identifier.</param>
    public Sale(Guid customerId, Guid branchId)
    {
        Id = Guid.NewGuid();
        SaleDate = DateTime.UtcNow;
        CustomerId = customerId;
        BranchId = branchId;
        Status = SaleStatus.Active;
    }

    /// <summary>
    /// Checks if a sale item already exists in the sale.
    /// </summary>
    /// <param name="item">Sale item to check.</param>
    /// <returns>True if the item exists, otherwise false.</returns>
    public bool SaleItemExists(SaleItem item) =>
        _items.Any(s => s.ProductId == item.ProductId);

    /// <summary>
    /// Adds an item to the sale.
    /// </summary>
    /// <param name="item">Sale item to add.</param>
    public void AddItem(SaleItem item)
    {
        if (item.Quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 items of the same product.");

        item.AssociateItem(Id);

        if (SaleItemExists(item))
        {
            var existingItem = _items.First(s => s.ProductId == item.ProductId);
            existingItem.AddUnits(item.Quantity);
        }
        else
        {
            _items.Add(item);
        }

        CalculateTotal();
    }

    /// <summary>
    /// Removes an item from the sale.
    /// </summary>
    /// <param name="item">Sale item to remove.</param>
    public void RemoveItem(SaleItem item)
    {
        _items.Remove(item);
        CalculateTotal();
    }

    /// <summary>
    /// Cancels the sale by updating its status.
    /// </summary>
    public void Cancel() => Status = SaleStatus.Cancelled;

    /// <summary>
    /// Calculates the total value of the sale.
    /// </summary>
    private void CalculateTotal() =>
        TotalValue = _items.Sum(item => item.TotalValue);
}