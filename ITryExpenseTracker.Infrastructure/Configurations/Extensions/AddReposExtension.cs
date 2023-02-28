using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Infrastructure.Repositories;

namespace ITryExpenseTracker.Infrastructure.Configurations.Extensions;

public static class AddReposExtension
{
    public static void AddRepos(this IServiceCollection services)
    {
        services.AddTransient<IExpenseRepo, ExpenseRepo>();

        services.AddTransient<ICategoryRepo, CategoryRepo>();

        services.AddTransient<ISupplierRepo, SupplierRepo>();
    }
}
