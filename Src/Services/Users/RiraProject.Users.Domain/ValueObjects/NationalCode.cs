using RiraProject.Framework.Domain.Results;
using RiraProject.Users.Domain.UserError;

namespace RiraProject.Users.Domain.ValueObjects;

public record NationalCode
{
    public string Value { get; }

    // سازنده private برای جلوگیری از ساخت مستقیم شیء بدون ولیدیشن
    private NationalCode(string value) => Value = value;

    public static Result<NationalCode> Create(string nationalCode)
    {
        if (string.IsNullOrWhiteSpace(nationalCode) || nationalCode.Length != 10 || !nationalCode.All(char.IsDigit))
            return Result.Failure<NationalCode>(UserErrors.InvalidNationalCodeFormat);

        if (!IsValidIranianNationalCode(nationalCode))
            return Result.Failure<NationalCode>(UserErrors.InvalidNationalCodeChecksum);

        return Result.Success(new NationalCode(nationalCode));
    }

    private static bool IsValidIranianNationalCode(string nationalCode)
    {
        var check = int.Parse(nationalCode[9].ToString());
        var sum = Enumerable.Range(0, 9)
            .Select(x => int.Parse(nationalCode[x].ToString()) * (10 - x))
            .Sum() % 11;

        return sum < 2 ? check == sum : check == 11 - sum;
    }

    public static implicit operator string(NationalCode code) => code.Value;
}
