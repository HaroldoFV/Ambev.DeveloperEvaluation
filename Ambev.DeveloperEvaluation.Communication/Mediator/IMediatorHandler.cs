using Ambev.DeveloperEvaluation.Messages;
using Ambev.DeveloperEvaluation.Messages.CommonMessages.DomainEvents;
using Ambev.DeveloperEvaluation.Messages.CommonMessages.Notifications;

namespace Ambev.DeveloperEvaluation.Communication.Mediator;

public interface IMediatorHandler
{
    Task PublishEventAsync<T>(T @event) where T : Event;
    Task<bool> SendCommandAsync<T>(T command) where T : Command;
    Task PublishNotificationAsync<T>(T notification) where T : DomainNotification;
    Task PublishDomainEventAsync<T>(T domainEvent) where T : DomainEvent;
}