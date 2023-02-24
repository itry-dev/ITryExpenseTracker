using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Infrastructure.Configurations.Extensions;
using ITryExpenseTracker.Scanner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(builder =>
{
    builder.AddConfiguration(
        new ConfigurationBuilder()        
        .AddUserSecrets<UserSecret>()
        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build());
        
});
builder.ConfigureServices((context, services) =>
{
    services.AddSingleton<IExpenseService, ExpenseService>();
    services.AddDbContext(context.Configuration.GetSection(nameof(UserSecret))["DefaultConnection"]);
});

using IHost host = builder.Build();

var expSvc = host.Services.GetRequiredService<IExpenseService>();
await expSvc.ScanAndCopyRecurringExpenses();

await host.RunAsync();

