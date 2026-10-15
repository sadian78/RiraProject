using RiraProject.Framework.Domain.Domain;
using RiraProject.Framework.Domain.Results;
using RiraProject.Users.Domain.ValueObjects;

namespace RiraProject.Users.Domain.Entities;

public class User : AggregateRoot<UserId>
{
    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public NationalCode NationalCode { get; private set; } = null!;
    public BirthDate BirthDate { get; private set; } = null!;

    // سازنده بدون پارامتر مورد نیاز EF Core
    private User() : base(default!) { }

    private User(UserId id, FirstName firstName, LastName lastName, NationalCode nationalCode, BirthDate birthDate)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        NationalCode = nationalCode;
        BirthDate = birthDate;
    }

    public static Result<User> Create(UserId id, FirstName firstName, LastName lastName, NationalCode nationalCode, BirthDate birthDate)
    {
        var user = new User(id, firstName, lastName, nationalCode, birthDate);
        return Result.Success(user);
    }

    public Result Update(FirstName firstName, LastName lastName, NationalCode nationalCode, BirthDate birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        NationalCode = nationalCode;
        BirthDate = birthDate;

        IncrementVersion();
        return Result.Success();
    }
}