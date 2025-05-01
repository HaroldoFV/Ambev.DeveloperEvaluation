using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.SeedWork;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM;

public class UnitOfWork
    : IUnitOfWork
{
    private readonly SaleDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(
        SaleDbContext context,
        ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        var aggregateRoots = _context.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entry => entry.Entity);

        _logger.LogInformation(
            "Committing changes for {Count} aggregate roots.",
            aggregateRoots.Count());

        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}