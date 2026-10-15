namespace RiraProject.Framework.Domain.Domain;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> Events { get; }
    void ClearDomainEvents();
}
