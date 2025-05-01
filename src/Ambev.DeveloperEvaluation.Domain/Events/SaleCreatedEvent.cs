using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.SeedWork;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCreatedEvent : DomainEvent
{
    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
    }

    public Sale Sale { get; }
}