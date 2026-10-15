namespace RiraProject.Framework.Domain.Contracts.Tools;

public interface IIdGenerator<TId>
{
    TId Next();
}
