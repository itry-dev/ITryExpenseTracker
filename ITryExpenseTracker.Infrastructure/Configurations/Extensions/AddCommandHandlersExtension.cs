using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Features.Categories.Queries.GetCategory;
using ITryExpenseTracker.Core.Features.Expenses.AddNew;
using ITryExpenseTracker.Core.Features.Expenses.Delete;
using ITryExpenseTracker.Core.Features.Expenses.Queries.GetSum;
using ITryExpenseTracker.Core.Features.Expenses.Update;
using ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpense;
using ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpenses;
using ITryExpenseTracker.Core.Features.Categories.Delete;
using ITryExpenseTracker.Core.Features.Suppliers.Queries.GetSuppliers;
using ITryExpenseTracker.Core.Features.Suppliers.AddNew;

namespace ITryExpenseTracker.Infrastructure.Configurations.Extensions;

public static class AddCommandHandlersExtension
{
    public static void AddCommandHandlers(this IServiceCollection services)
    {
        #region expense command, handler
        services.AddMediatR(
        typeof(AddNewExpenseCommand),
        typeof(AddNewExpenseCommandHandler));
        services.AddScoped<IValidator<AddNewExpenseCommand>, AddNewExpenseCommandValidator>();

        services.AddMediatR(
        typeof(UpdateExpenseCommand),
        typeof(UpdateExpenseCommandHandler));
        services.AddScoped<IValidator<UpdateExpenseCommand>, UpdateExpenseCommandValidator>();

        services.AddMediatR(
        typeof(DeleteExpenseCommand),
        typeof(DeleteExpenseCommandHandler));
        services.AddScoped<IValidator<DeleteExpenseCommand>, DeleteExpenseCommandValidator>();

        services.AddMediatR(
        typeof(GetExpenseCommand),
        typeof(GetExpenseCommandHandler));
        services.AddScoped<IValidator<GetExpenseCommand>, GetExpenseCommandValidator>();

        services.AddMediatR(
        typeof(GetExpensesCommand),
        typeof(GetExpensesCommandHandler));
        services.AddScoped<IValidator<GetExpensesCommand>, GetExpensesCommandValidator>();

        services.AddMediatR(
        typeof(GetExpensesSlimCommand),
        typeof(GetExpensesSlimCommandHandler));
        services.AddScoped<IValidator<GetExpensesSlimCommand>, GetExpensesSlimCommandValidator>();

        services.AddMediatR(
        typeof(GetSumCommand),
        typeof(GetSumCommandHandler));
        services.AddScoped<IValidator<GetSumCommand>, GetSumCommandValidator>();
        #endregion

        #region cagory command, handler
        services.AddMediatR(
        typeof(GetCategoriesCommand),
        typeof(GetCategoriesCommandHandler));

        services.AddMediatR(
        typeof(DeleteCategoryCommand),
        typeof(DeleteCategoryCommand));
        services.AddScoped<IValidator<DeleteCategoryCommand>, DeleteCategoryCommandValidator>();
        #endregion

        #region supplier command, handler
        services.AddMediatR(
        typeof(GetSuppliersCommand),
        typeof(GetSuppliersCommandHandler));
        services.AddScoped<IValidator<GetSuppliersCommand>, GetSuppliersCommandValidator>();

        services.AddMediatR(
        typeof(AddNewSupplierCommand),
        typeof(AddNewSupplierCommandHandler));
        services.AddScoped<IValidator<AddNewSupplierCommand>, AddNewSupplierCommandValidator>();
        #endregion

    }
}
