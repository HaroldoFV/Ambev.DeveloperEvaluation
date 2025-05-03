using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.SeedWork;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCancelledEvent : DomainEvent
{
    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
    }

    public Sale Sale { get; }
}