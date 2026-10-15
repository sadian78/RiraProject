using RiraProject.Framework.Domain.Contracts.Repositories;
using RiraProject.Users.Domain.Entities;
using RiraProject.Users.Domain.ValueObjects;

namespace RiraProject.Users.Domain.Repository;

public interface IUserRepository : IGenericRepository<User, UserId>
{
    // فقط متدهای اختصاصی User که در Generic وجود ندارند:
    Task<User?> GetByNationalCodeAsync(NationalCode nationalCode, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNationalCodeAsync(NationalCode nationalCode, CancellationToken cancellationToken = default);
}

