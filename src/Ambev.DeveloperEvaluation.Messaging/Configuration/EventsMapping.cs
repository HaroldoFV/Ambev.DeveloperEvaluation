using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Messaging.Configuration;

internal static class EventsMapping
{
    private static Dictionary<string, string> _routingKeys => new()
    {
        { nameof(SaleCreatedEvent), "sale.created" },
        { nameof(SaleModifiedEvent), "sale.modified" }
    };

    public static string GetRoutingKey<T>() => _routingKeys[typeof(T).Name];
}