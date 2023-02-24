using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Infrastructure.Models;

namespace ITryExpenseTracker.Infrastructure.Configurations.Extensions;

public static class AddDbContextExtension
{
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        services.AddDbContext<DataContext>(options =>
        {
            var serverVersion = new MySqlServerVersion(ITryExpenseTracker.Core.Constants.MySqlServer.GetVersion());
            options.UseMySql(connectionString, serverVersion, opt => opt.EnableRetryOnFailure(1));

#if DEBUG
            options.LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
#endif
        });

        services.AddIdentityCore<ApplicationUser>(
            options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            }
        )
            .AddRoles<ApplicationUserRole>()
            .AddEntityFrameworkStores<DataContext>(); ;

    }
}
