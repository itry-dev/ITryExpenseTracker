using ITryExpenseTracker.User.Abstractions.Services;
using ITryExpenseTracker.User.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ITryExpenseTracker.User.Infrastructure.Configuration;

public static class AddUserServicesExtension
{
    public static void AddUserServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
    }
}
