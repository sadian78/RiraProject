using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiraProject.Framework.Domain.Contracts;

namespace RiraProject.Framework.Infrastructure.Persistance.Configuration;

/// <summary>
/// کلاس پایه برای تنظیمات کانفیگ EF Core تمام موجودیت‌های پروژه
/// </summary>
public abstract class BaseEntityTypeConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity<TId>
    where TId : notnull
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // تنظیمات عمومی همه موجودیت‌ها (مثلاً کلید اصلی)
        builder.HasKey(e => e.Id);
    }
}