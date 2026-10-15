using RiraProject.Framework.Domain.Results;

namespace RiraProject.Users.Domain.UserError;
public static class UserErrors
{
    public static readonly Error NotFound =
        Error.NotFound("User.NotFound", "کاربری با این مشخصات یافت نشد.");

    public static readonly Error EmptyFirstName =
        Error.Validation("User.EmptyFirstName", "نام نمی‌تواند خالی باشد.");

    public static readonly Error EmptyLastName =
        Error.Validation("User.EmptyLastName", "نام خانوادگی نمی‌تواند خالی باشد.");

    public static readonly Error InvalidNationalCodeFormat =
        Error.Validation("User.InvalidNationalCodeFormat", "کد ملی باید دقیقا ۱۰ رقم عددی باشد.");

    public static readonly Error InvalidNationalCodeChecksum =
        Error.Validation("User.InvalidNationalCodeChecksum", "کد ملی وارد شده نامعتبر است.");

    public static readonly Error NationalCodeAlreadyExists =
        Error.Conflict("User.NationalCodeAlreadyExists", "کاربری با این کد ملی از قبل ثبت شده است.");

    public static readonly Error FutureBirthDate =
        Error.Validation("User.FutureBirthDate", "تاریخ تولد نمی‌تواند در آینده باشد.");
}