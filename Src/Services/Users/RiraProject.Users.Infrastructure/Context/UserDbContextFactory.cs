using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RiraProject.Users.Infrastructure.Context;

public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();

        // همان رشته اتصالی که قبلاً با Windows Authentication تنظیم کردید:
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=RiraUserDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=True");

        return new UserDbContext(optionsBuilder.Options);
    }
}