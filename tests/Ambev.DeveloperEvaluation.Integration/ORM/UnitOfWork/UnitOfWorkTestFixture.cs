using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.@base;
using Bogus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.ORM.UnitOfWork;

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection
    : ICollectionFixture<UnitOfWorkTestFixture>
{
}

public class UnitOfWorkTestFixture
    : BaseFixture
{
    public int GetValidSaleQuantity()
        => Faker.Random.Int(1, 100);

    public decimal GetValidSalePrice()
        => Faker.Random.Decimal(10, 1000);

    public Sale GetExampleSale()
        => new(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

    public List<Sale> GetExampleSalesList(int length = 10)
        => Enumerable.Range(1, length)
            .Select(_ => GetExampleSale()).ToList();
}