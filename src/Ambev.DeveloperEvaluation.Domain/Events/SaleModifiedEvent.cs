using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.SeedWork;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent : DomainEvent
{
    public SaleModifiedEvent(Sale sale)
    {
        Sale = sale;
    }

    public Sale Sale { get; }
}