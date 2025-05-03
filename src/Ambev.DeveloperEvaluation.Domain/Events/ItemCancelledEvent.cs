using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.SeedWork;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ItemCancelledEvent : DomainEvent
{
    public SaleItem Item { get; }
    public Sale Sale { get; }

    public ItemCancelledEvent(Sale sale, SaleItem item)
    {
        Sale = sale;
        Item = item;
    }
}