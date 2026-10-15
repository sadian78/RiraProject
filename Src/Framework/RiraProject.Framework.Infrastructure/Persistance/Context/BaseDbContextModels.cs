using Microsoft.EntityFrameworkCore;
using RiraProject.Framework.Application.Contracts.ManagedTransaction;

namespace RiraProject.Framework.Infrastructure.Persistance.Context;

public abstract class BaseDbContextModels : DbContext, IUnitOfWork
{
    protected BaseDbContextModels(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // به طور خودکار تمام IEntityTypeConfigurationهای موجود در اسمبلی لایه متقاضی را اسکن و اعمال می‌کند
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    //اگه بعدا نیاز داشتیم که قبل Savechange چیزی پیاده کنیم که برای همه اعمال بشه
    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
