using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Messages;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public abstract class BaseEntity : IComparable<BaseEntity>
{
    public Guid Id { get; set; }

    private readonly List<Event> _notifications = new();
    public IReadOnlyCollection<Event> Notifications => _notifications.AsReadOnly();

    public async Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return await Validator.ValidateAsync(this);
    }

    public int CompareTo(BaseEntity? other)
    {
        return other == null ? 1 : Id.CompareTo(other.Id);
    }

    public void AddEvent(Event @event)
    {
        _notifications.Add(@event);
    }

    public void RemoveEvent(Event @event)
    {
        _notifications.Remove(@event);
    }

    public void ClearEvents()
    {
        _notifications.Clear();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity other) return false;
        if (ReferenceEquals(this, other)) return true;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(BaseEntity? a, BaseEntity? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity? a, BaseEntity? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }
}