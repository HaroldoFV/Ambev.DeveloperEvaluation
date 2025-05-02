using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.SeedWork;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM;

public class UnitOfWork
    : IUnitOfWork
{
    private readonly SaleDbContext _context;
    private readonly IDomainEventPublisher _publisher;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(
        SaleDbContext context,
        IDomainEventPublisher publisher,
        ILogger<UnitOfWork> logger)
    {
        _context = context;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        var aggregateRoots = _context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entry => entry.Entity.Events.Any())
            .Select(entry => entry.Entity);

        var aggregates = aggregateRoots as AggregateRoot[] ?? aggregateRoots.ToArray();
        _logger.LogInformation(
            "Commit: {AggregatesCount} aggregate roots with events.",
            aggregates.Count());

        var events = aggregates
            .SelectMany(aggregate => aggregate.Events);

        var domainEvents = events as DomainEvent[] ?? events.ToArray();
        _logger.LogInformation(
            "Commit: {EventsCount} events raised.", domainEvents.Count());

        foreach (var @event in domainEvents)
            await _publisher.PublishAsync((dynamic)@event, cancellationToken);

        foreach (var aggregate in aggregates)
            aggregate.ClearEvents();

        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}