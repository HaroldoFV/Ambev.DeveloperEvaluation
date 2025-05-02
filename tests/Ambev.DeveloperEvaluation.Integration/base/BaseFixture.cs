using Ambev.DeveloperEvaluation.ORM;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Integration.@base;

public class BaseFixture
{
    public BaseFixture()
        => Faker = new Faker("pt_BR");

    protected Faker Faker { get; set; }

    public SaleDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new SaleDbContext(
            new DbContextOptionsBuilder<SaleDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );
        if (preserveData == false)
            context.Database.EnsureDeleted();
        return context;
    }
}