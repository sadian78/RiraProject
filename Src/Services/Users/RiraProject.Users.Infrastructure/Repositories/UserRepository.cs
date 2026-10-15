using Microsoft.EntityFrameworkCore;
using RiraProject.Framework.Infrastructure.Persistence.Repositories;
using RiraProject.Users.Domain.Entities;
using RiraProject.Users.Domain.Repository;
using RiraProject.Users.Domain.ValueObjects;
using RiraProject.Users.Infrastructure.Context;

namespace RiraProject.Users.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User, UserId>, IUserRepository
{
    private readonly UserDbContext _userDbContext;

    public UserRepository(UserDbContext context) : base(context)
    {
        _userDbContext = context;
    }

    public async Task<User?> GetByNationalCodeAsync(NationalCode nationalCode, CancellationToken cancellationToken = default)
    {
        return await _userDbContext.Users
            .FirstOrDefaultAsync(u => u.NationalCode == nationalCode, cancellationToken);
    }

    public async Task<bool> ExistsByNationalCodeAsync(NationalCode nationalCode, CancellationToken cancellationToken = default)
    {
        return await _userDbContext.Users
            .AnyAsync(u => u.NationalCode == nationalCode, cancellationToken);
    }
}