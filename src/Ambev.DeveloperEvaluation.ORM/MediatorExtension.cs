using Ambev.DeveloperEvaluation.Communication.Mediator;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.ORM;

public static class MediatorExtension
{
    public static async Task PublishEventsAsync(this IMediatorHandler mediator, SaleContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(entry => entry.Entity.Notifications.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(entry => entry.Entity.Notifications)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearEvents());

        var tasks = domainEvents
            .Select(async domainEvent
                =>
            {
                await mediator.PublishEventAsync(domainEvent);
            });

        await Task.WhenAll(tasks);
    }
}