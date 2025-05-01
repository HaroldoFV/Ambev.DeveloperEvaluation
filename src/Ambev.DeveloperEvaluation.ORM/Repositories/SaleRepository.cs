using Ambev.DeveloperEvaluation.Application.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly SaleDbContext _dbContext;

    public SaleRepository(SaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await _dbContext.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        NotFoundException.ThrowIfNull(sale, $"Sale '{id}' not found.");

        return sale!;
    }

    public Task DeleteAsync(Sale sale, CancellationToken cancellationToken)
    {
        _dbContext.Sales.Remove(sale);
        return Task.CompletedTask;
    }

    // public async Task<IEnumerable<Sale>> GetAllReadOnlyAsync(CancellationToken cancellationToken = default)
    // {
    //     return await _dbContext.Sales
    //         .Include(s => s.Items)
    //         .AsNoTracking()
    //         .ToListAsync(cancellationToken);
    // }

    public async Task CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _dbContext.Sales.AddAsync(sale, cancellationToken);
    }

    public Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_dbContext.Sales.Update(sale));
    }

    public Task<SearchOutput<Sale>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}