using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Mapper.Services;

namespace ITryExpenseTracker.Mapper.Configurations.Extensions;

public static class AddModelMapperExtension
{
    public static void AddModelMappers(this IServiceCollection services)
    {
        services.AddTransient<IExpenseMapperService, ExpenseMapperService>();
        services.AddTransient<ICategoryMapperService, CategoryMapperService>();
        services.AddTransient<ISupplierMapperService, SupplierMapperService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
