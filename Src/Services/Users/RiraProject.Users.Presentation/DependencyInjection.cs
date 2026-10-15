using Microsoft.Extensions.DependencyInjection;
using RiraProject.Users.Presentation.Interceptors;

namespace RiraProject.Users.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersPresentation(this IServiceCollection services)
    {
        // اینترسپتور مدیریت سراسری خطاها در gRPC
        services.AddTransient<GrpcExceptionInterceptor>();

        // تنظیمات gRPC همراه با Interceptor
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<GrpcExceptionInterceptor>();
        });

        return services;
    }
}