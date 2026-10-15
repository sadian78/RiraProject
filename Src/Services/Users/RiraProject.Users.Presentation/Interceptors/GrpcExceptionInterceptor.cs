using Grpc.Core;
using Grpc.Core.Interceptors;

namespace RiraProject.Users.Presentation.Interceptors;

public class GrpcExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException)
        {
            // اگر خطای از پیش فرمت‌شده gRPC باشد مستقیماً رد می‌شود
            throw;
        }
        catch (Exception ex)
        {
            // خطاهای پیش‌بینی نشده سرور را کپسوله کرده و به RpcException داخلی تبدیل می‌کند
            throw new RpcException(new Status(StatusCode.Internal, $"خطای غیرمنتظره سرور رخ داده است: {ex.Message}"));
        }
    }
}
