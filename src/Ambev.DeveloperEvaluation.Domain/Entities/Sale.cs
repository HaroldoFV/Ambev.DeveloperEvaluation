using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity, IAggregateRoot
{
    public long SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }
    public Guid CustomerId { get; private set; } // External Identity
    public Guid BranchId { get; private set; } // External Identity
    private readonly List<SaleItem> _items;
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
    public decimal TotalValue { get; private set; }
    public bool IsCancelled { get; private set; }

    public Sale(Guid customerId, Guid branchId)
    {
        Id = Guid.NewGuid();
        SaleDate = DateTime.UtcNow;
        CustomerId = customerId;
        BranchId = branchId;
        _items = new List<SaleItem>();
    }

    public void AddItem(SaleItem item)
    {
        if (item.Quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 items of the same product.");

        item.AssociateItem(Id);

        _items.Add(item);
        CalculateTotal();
    }

    public void RemoveItem(SaleItem item)
    {
        _items.Remove(item);
        CalculateTotal();
    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    private void CalculateTotal()
    {
        TotalValue = _items.Sum(item => item.TotalValue);
    }
}