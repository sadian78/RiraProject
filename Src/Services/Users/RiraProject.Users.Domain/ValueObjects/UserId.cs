using RiraProject.Framework.Domain.ValueObject;

namespace RiraProject.Users.Domain.ValueObjects;

public record UserId(Guid Value) : BaseId<Guid>(Value)
{
    public static UserId New() => new(Guid.NewGuid());
    public static UserId From(Guid value) => new(value);
}
