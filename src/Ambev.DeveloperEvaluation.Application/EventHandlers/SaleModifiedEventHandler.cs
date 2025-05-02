using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.SeedWork;

namespace Ambev.DeveloperEvaluation.Application.EventHandlers;

public class SaleModifiedEventHandler : IDomainEventHandler<SaleModifiedEvent>
{
    private readonly IMessageProducer _messageProducer;

    public SaleModifiedEventHandler(IMessageProducer messageProducer)
    {
        _messageProducer = messageProducer;
    }

    public async Task HandleAsync(SaleModifiedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _messageProducer.SendMessageAsync(domainEvent, cancellationToken);
    }
}