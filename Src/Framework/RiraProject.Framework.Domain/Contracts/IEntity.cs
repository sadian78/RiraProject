namespace RiraProject.Framework.Domain.Contracts;

public interface IEntity<TId>
    where TId : notnull
{
    TId Id { get; }
}
