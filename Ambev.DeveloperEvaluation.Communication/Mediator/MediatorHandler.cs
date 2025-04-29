using Ambev.DeveloperEvaluation.Messages;
using Ambev.DeveloperEvaluation.Messages.CommonMessages.DomainEvents;
using Ambev.DeveloperEvaluation.Messages.CommonMessages.Notifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEventAsync<T>(T @event) where T : Event
        {
            await _mediator.Publish(@event);
        }

        public async Task<bool> SendCommandAsync<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }

        public async Task PublishNotificationAsync<T>(T notification) where T : DomainNotification
        {
            await _mediator.Publish(notification);
        }

        public async Task PublishDomainEventAsync<T>(T domainEvent) where T : DomainEvent
        {
            await _mediator.Publish(domainEvent);
        }
    }
}