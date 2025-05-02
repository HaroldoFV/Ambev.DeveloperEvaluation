using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.SeedWork;

namespace Ambev.DeveloperEvaluation.Application.EventHandlers;

public class SaleCancelledEventHandler : IDomainEventHandler<SaleCancelledEvent>
{
    private readonly IMessageProducer _messageProducer;

    public SaleCancelledEventHandler(IMessageProducer messageProducer)
    {
        _messageProducer = messageProducer;
    }

    public async Task HandleAsync(SaleCancelledEvent domainEvent, CancellationToken cancellationToken)
    {
        await _messageProducer.SendMessageAsync(domainEvent, cancellationToken);
    }
}