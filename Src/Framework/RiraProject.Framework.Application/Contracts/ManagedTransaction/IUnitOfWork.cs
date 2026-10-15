namespace RiraProject.Framework.Application.Contracts.ManagedTransaction;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
