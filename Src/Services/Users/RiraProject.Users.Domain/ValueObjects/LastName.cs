using RiraProject.Framework.Domain.Results;
using RiraProject.Users.Domain.UserError;

namespace RiraProject.Users.Domain.ValueObjects;

public record LastName
{
    public string Value { get; }

    private LastName(string value) => Value = value;

    public static Result<LastName> Create(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<LastName>(UserErrors.EmptyLastName);

        var trimmed = lastName.Trim();
        if (trimmed.Length is < 2 or > 50)
            return Result.Failure<LastName>(Error.Validation("LastName.Length", "طول نام خانوادگی باید بین ۲ تا ۵۰ کاراکتر باشد."));

        return Result.Success(new LastName(trimmed));
    }

    public static implicit operator string(LastName lastName) => lastName.Value;
}
