using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiraProject.Users.Domain.Entities;
using RiraProject.Users.Domain.ValueObjects;

namespace RiraProject.Users.Infrastructure.Persistance.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => UserId.From(value))
            .ValueGeneratedNever();

        builder.Property(u => u.FirstName)
            .HasConversion(name => name.Value, value => FirstName.Create(value).Value)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasConversion(name => name.Value, value => LastName.Create(value).Value)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.NationalCode)
            .HasConversion(code => code.Value, value => NationalCode.Create(value).Value)
            .HasMaxLength(10)
            .IsFixedLength()
            .IsRequired();

        builder.HasIndex(u => u.NationalCode)
            .IsUnique();

        builder.Property(u => u.BirthDate)
            .HasConversion(date => date.Value, value => BirthDate.Create(value).Value)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(u => u.Version)
            .IsConcurrencyToken();

        builder.Ignore(u => u.Events);
    }
}