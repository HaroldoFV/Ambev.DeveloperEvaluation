using System.Text.Json;
using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Messaging.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Ambev.DeveloperEvaluation.Messaging.Producer;

public class RabbitMQProducer : IMessageProducer
{
    private readonly IModel _channel;
    private readonly string _exchange;

    public RabbitMQProducer(IModel channel,
        IOptions<RabbitMQConfiguration> options)
    {
        _channel = channel;
        _exchange = options.Value.Exchange!;
    }

    public Task SendMessageAsync<T>(T message, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var routingKey = EventsMapping.GetRoutingKey<T>();
        var @event = JsonSerializer.SerializeToUtf8Bytes(message, options);
        _channel.BasicPublish(
            exchange: _exchange,
            routingKey: routingKey,
            body: @event);
        _channel.WaitForConfirmsOrDie();
        return Task.CompletedTask;
    }
}