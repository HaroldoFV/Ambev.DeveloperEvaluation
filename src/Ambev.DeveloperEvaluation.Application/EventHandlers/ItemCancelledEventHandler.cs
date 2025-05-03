using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.SeedWork;

namespace Ambev.DeveloperEvaluation.Application.EventHandlers;

public class ItemCancelledEventHandler : IDomainEventHandler<ItemCancelledEvent>
{
    private readonly IMessageProducer _messageProducer;

    public ItemCancelledEventHandler(IMessageProducer messageProducer)
    {
        _messageProducer = messageProducer;
    }

    public async Task HandleAsync(ItemCancelledEvent domainEvent, CancellationToken cancellationToken)
    {
        await _messageProducer.SendMessageAsync(domainEvent, cancellationToken);
    }
}