using RiraProject.Framework.Domain.Contracts;

namespace RiraProject.Framework.Domain.Domain;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : notnull
{
    public int Version { get; private set; }
    private bool _isIncremented;

    // مقداردهی اولیه برای رفع CS8618 و جلوگیری از NullReferenceException
    private readonly List<IDomainEvent> _events = new();

    // پیاده‌سازی اینترفیس IAggregateRoot به صورت ReadOnly
    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    protected AggregateRoot(TId id) : base(id)
    {
    }

    protected void IncrementVersion()
    {
        if (_isIncremented) return;

        Version++;
        _isIncremented = true;
    }

    public void ResetVersionIncrementFlag()
    {
        _isIncremented = false;
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        if (!_events.Any() && !_isIncremented)
        {
            Version++;
            _isIncremented = true;
        }

        _events.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}