using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Domain.SeedWork;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.ORM.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(CommitAsync))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task CommitAsync()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleSalesList = _fixture.GetExampleSalesList();
        var saleWithEvent = exampleSalesList.First();
        var @event = new DomainEventFake();
        saleWithEvent.RaiseEvent(@event);
        var eventHandlerMock = new Mock<IDomainEventHandler<DomainEventFake>>();
        await dbContext.AddRangeAsync(exampleSalesList);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddSingleton(eventHandlerMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new DeveloperEvaluation.ORM.UnitOfWork(dbContext,
            eventPublisher,
            serviceProvider.GetRequiredService<ILogger<DeveloperEvaluation.ORM.UnitOfWork>>());

        await unitOfWork.CommitAsync(CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var savedSales = assertDbContext.Sales
            .AsNoTracking().ToList();
        savedSales.Should()
            .HaveCount(exampleSalesList.Count);
        eventHandlerMock.Verify(x =>
                x.HandleAsync(@event, It.IsAny<CancellationToken>()),
            Times.Once);
        saleWithEvent.Events.Should().BeEmpty();
    }


    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Rollback()
    {
        var dbContext = _fixture.CreateDbContext();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var eventPublisher = new DomainEventPublisher(serviceProvider);
        var unitOfWork = new DeveloperEvaluation.ORM.UnitOfWork(dbContext,
            eventPublisher,
            serviceProvider.GetRequiredService<ILogger<DeveloperEvaluation.ORM.UnitOfWork>>());

        var task = async ()
            => await unitOfWork.RollbackAsync(CancellationToken.None);

        await task.Should().NotThrowAsync();
    }
}