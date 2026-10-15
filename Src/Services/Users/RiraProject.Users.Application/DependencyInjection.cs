using Microsoft.Extensions.DependencyInjection;
using RiraProject.Users.Application.Services;

namespace RiraProject.Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersApplication(this IServiceCollection services)
    {
        // ثبت سرویس‌های دامنه کاربر به صورت Scoped
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
