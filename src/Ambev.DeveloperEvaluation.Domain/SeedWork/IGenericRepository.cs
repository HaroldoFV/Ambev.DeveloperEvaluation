namespace Ambev.DeveloperEvaluation.Domain.SeedWork;

public interface IGenericRepository<TAggregate> : IRepository
    where TAggregate : AggregateRoot
{
    public Task CreateAsync(TAggregate aggregate, CancellationToken cancellationToken);
    public Task<TAggregate> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken);
    public Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken);
}