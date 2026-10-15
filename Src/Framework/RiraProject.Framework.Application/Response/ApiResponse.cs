using RiraProject.Framework.Domain.Results;

namespace RiraProject.Framework.Application.Contracts.Response;

public class ApiResponse
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected ApiResponse(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    // متدهای ساخت پاسخ موفق و ناموفق بدون دیتای بازگشتی
    public static ApiResponse Success()
        => new(true, null);

    public static ApiResponse Failure(Error error)
        => new(false, error);

    // متدهای ساخت پاسخ برای حالت‌های دارای دیتا
    public static ApiResponse<T> Success<T>(T data)
        => ApiResponse<T>.Success(data);

    public static ApiResponse<T> Failure<T>(Error error)
        => ApiResponse<T>.Failure(error);
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; }

    private ApiResponse(bool isSuccess, T? data, Error? error)
        : base(isSuccess, error)
    {
        Data = data;
    }

    public static ApiResponse<T> Success(T data)
        => new(true, data, null);

    public new static ApiResponse<T> Failure(Error error)
        => new(false, default, error);
}