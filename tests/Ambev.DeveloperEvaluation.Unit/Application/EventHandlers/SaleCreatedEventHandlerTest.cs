using Ambev.DeveloperEvaluation.Application.EventHandlers;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.EventHandlers;

public class SaleCreatedEventHandlerTest
{
    [Fact(DisplayName = nameof(HandleAsync))]
    [Trait("Application", "EventHandlers")]
    public async Task HandleAsync()
    {
        // Arrange
        var messageProducerMock = new Mock<IMessageProducer>();
        messageProducerMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<SaleCreatedEvent>(),
                It.IsAny<CancellationToken>()
            ))
            .Returns(Task.CompletedTask);

        var handler = new SaleCreatedEventHandler(messageProducerMock.Object);
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var @event = new SaleCreatedEvent(sale);

        // Act
        await handler.HandleAsync(@event, CancellationToken.None);

        // Assert
        messageProducerMock
            .Verify(x => x.SendMessageAsync(@event, CancellationToken.None),
                Times.Once);
    }
}