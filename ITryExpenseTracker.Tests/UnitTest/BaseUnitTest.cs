using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Infrastructure;
using ITryExpenseTracker.Infrastructure.Repositories;
using ITryExpenseTracker.Scanner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Identity;
using ITryExpenseTracker.Infrastructure.Models;

namespace ITryExpenseTracker.Tests.UnitTest;

public class BaseUnitTest : BaseTestClass, IDisposable
{
    protected DataContext DbContext;

    private IExpenseService _expenseService;

    private ICategoryRepo _categoryRepo;

    private IExpenseRepo _expenseRepo;

    protected ILoggerFactory LoggerFactory = new NullLoggerFactory();

    protected const string ADMIN_ID = "2f77f509-4f9d-4dbc-b624-9a79a3b983ef";

    protected static Guid DEFAULT_CATEGORY_ID = Guid.Parse("88c7aaa9-261a-43bc-8f75-9b64606c6fe4");

    public BaseUnitTest()
    {
        var builder = new DbContextOptionsBuilder<DataContext>();
        builder.UseInMemoryDatabase("expensetracker")
            .EnableSensitiveDataLogging(true)
            .EnableDetailedErrors(true);
        DbContext = new DataContext(builder.Options);

        _seedData();
    }

    #region _seedData
    private void _seedData()
    {
        var u = new Infrastructure.Models.ApplicationUser
        {
            Id = ADMIN_ID,
            Name = "admin",
            Email = "foo@bar.com",
            EmailConfirmed = true
        };

        u.PasswordHash = new PasswordHasher<Infrastructure.Models.ApplicationUser>().HashPassword(u, "password");

        var role = new Infrastructure.Models.ApplicationUserRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "admin",
            NormalizedName = "admin"
        };

        DbContext.Roles.Remove(role);
        DbContext.Roles.Add(role);

        DbContext.Users.Remove(u);
        DbContext.Users.Add(u);

        DbContext.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
        {
            RoleId = role.Id,
            UserId = u.Id
        });

        var c = new Category
        {
            Id = DEFAULT_CATEGORY_ID,
            Name = "General"
        };
        DbContext.Categories.Remove(c);
        DbContext.Categories.Add(c);

        DbContext.SaveChanges();
    }
    #endregion

    #region GetExpenseService
    protected IExpenseService GetExpenseService()
    {
        if (_expenseService == null)
        {
            _expenseService = new ExpenseService(DbContext, LoggerFactory);
        }

        return _expenseService;
    }
    #endregion

    #region GetExpenseRepo
    protected IExpenseRepo GetExpenseRepo()
    {
        if (_expenseRepo == null)
        {
            _expenseRepo = new ExpenseRepo(DbContext, LoggerFactory, GetCategoryRepo(), GetExpenseMapperService());
        }

        return _expenseRepo;
    }
    #endregion

    #region GetCategoryRepo
    protected ICategoryRepo GetCategoryRepo()
    {
        if (_categoryRepo == null)
        {
            _categoryRepo = new CategoryRepo(DbContext, LoggerFactory, GetCategoryMapperService());
        }

        return _categoryRepo;
    }
    #endregion


    public void Dispose() {
        DbContext.Expenses.RemoveRange(DbContext.Expenses);
        DbContext.UserRoles.RemoveRange(DbContext.UserRoles);
        DbContext.Users.RemoveRange(DbContext.Users);
        DbContext.Roles.RemoveRange(DbContext.Roles);
        DbContext.Categories.RemoveRange(DbContext.Categories);
        DbContext.SaveChanges();
    }
}
