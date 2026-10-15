using RiraProject.Framework.Domain.Results;
using RiraProject.Users.Domain.UserError;

namespace RiraProject.Users.Domain.ValueObjects;

public record BirthDate
{
    public DateTime Value { get; }

    private BirthDate(DateTime value)
    {
        Value = value;
    }

    public static Result<BirthDate> Create(DateTime birthDate)
    {
        if (birthDate == default)
            return Result.Failure<BirthDate>(Error.Validation("BirthDate.Empty", "تاریخ تولد باید مشخص شده باشد."));

        if (birthDate.Date > DateTime.UtcNow.Date)
            return Result.Failure<BirthDate>(UserErrors.FutureBirthDate);

        if (birthDate.Date < DateTime.UtcNow.AddYears(-130).Date)
            return Result.Failure<BirthDate>(Error.Validation("BirthDate.InvalidRange", "تاریخ تولد وارد شده معتبر نیست."));

        return Result.Success(new BirthDate(birthDate.Date));
    }

    public static implicit operator DateTime(BirthDate birthDate) => birthDate.Value;
}