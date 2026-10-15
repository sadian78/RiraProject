using global::RiraProject.Framework.Domain.Results;
using global::RiraProject.Users.Domain.UserError;

namespace RiraProject.Users.Domain.ValueObjects;
public record FirstName
{
    public string Value { get; }

    private FirstName(string value) => Value = value;

    public static Result<FirstName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<FirstName>(UserErrors.EmptyFirstName);

        var trimmed = firstName.Trim();
        if (trimmed.Length is < 2 or > 50)
            return Result.Failure<FirstName>(Error.Validation("FirstName.Length", "طول نام باید بین ۲ تا ۵۰ کاراکتر باشد."));

        return Result.Success(new FirstName(trimmed));
    }

    public static implicit operator string(FirstName firstName) => firstName.Value;
}
