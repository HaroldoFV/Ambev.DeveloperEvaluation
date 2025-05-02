using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.SeedWork;

namespace Ambev.DeveloperEvaluation.Application.EventHandlers;

public class SaleCreatedEventHandler : IDomainEventHandler<SaleCreatedEvent>
{
    private readonly IMessageProducer _messageProducer;

    public SaleCreatedEventHandler(IMessageProducer messageProducer)
    {
        _messageProducer = messageProducer;
    }

    public async Task HandleAsync(SaleCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _messageProducer.SendMessageAsync(domainEvent, cancellationToken);
    }
}