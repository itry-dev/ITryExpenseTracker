using ITryExpenseTracker.User.Abstractions;
using ITryExpenseTracker.User.Abstractions.Services;
using ITryExpenseTracker.User.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITryExpenseTracker.User.Infrastructure.Configuration;

public static class AddUserServicesExtension
{
    public static void AddUserServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddOptions<EmailConfiguration>()
                .Configure<IConfiguration>((settings, configuration) => {
                    var root = "EmailConfiguration";

                    settings.SmtpServer = configuration.GetValue<string>($"{root}:SmtpServer");
                    settings.SmtpUser = configuration.GetValue<string>($"{root}:SmtpUser");
                    settings.SmtpPassword = configuration.GetValue<string>($"{root}:SmtpPassword");
                    settings.SmtpPort = configuration.GetValue<int>($"{root}:SmtpPort");
                    settings.MailFrom = configuration.GetValue<string>($"{root}:MailFrom");

                });
    }


}
