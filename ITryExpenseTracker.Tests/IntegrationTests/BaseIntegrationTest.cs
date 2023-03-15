using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Authentication.Abstractions.Abstractions;
using ITryExpenseTracker.Core.Authentication.Abstractions.InputModels;
using ITryExpenseTracker.Core.Authentication.Abstractions.OutputModels;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using ITryExpenseTracker.Infrastructure;
using ITryExpenseTracker.Infrastructure.Models;
using ITryExpenseTracker.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ITryExpenseTracker.Tests.IntegrationTests;

public class BaseIntegrationTest
    : BaseTestClass, IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly HttpClient HttpClient;
    protected readonly CustomWebApplicationFactory<Program> Factory;

    protected readonly string ADMIN_USER_NAME = "admin";

    protected readonly string ADMIN_USER_PASSWORD = "password";

    protected readonly Guid ADMIN_ID = Guid.Parse("6dd09a10-9f26-4f38-ac94-376a52217b4d");

    protected readonly IUserService UserService;
    protected readonly IExpenseRepo ExpenseRepo;
    protected readonly ICategoryRepo CategoryRepo;
    protected readonly ISupplierRepo SupplierRepo;
    protected readonly DataContext TheDataContext;

    public BaseIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        Factory = factory;

        HttpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var scope = Factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        UserService = scopedServices.GetRequiredService<IUserService>();
        ExpenseRepo = scopedServices.GetRequiredService<IExpenseRepo>();
        CategoryRepo = scopedServices.GetRequiredService<ICategoryRepo>();
        SupplierRepo = scopedServices.GetRequiredService<ISupplierRepo>();
        TheDataContext = scopedServices.GetRequiredService<DataContext>();

        _seedDb().GetAwaiter().GetResult();
    }

    #region _seedDb
    private async Task _seedDb()
    {
        await ClearDbData()
            .ConfigureAwait(false);

        await AddUserWithRole(ADMIN_ID, ADMIN_USER_NAME, ADMIN_USER_PASSWORD, ITryExpenseTracker.Core.Constants.UserRoles.ADMIN)
            .ConfigureAwait(false);

        await AddNewCategoryToDb("General")
            .ConfigureAwait(false);
    }
    #endregion

    #region AddUserWithRole
    protected async Task<ApplicationUser> AddUserWithRole(Guid userId, string username, string password, string role)
    {
        _checkUserRole(role);

        var dbUser = await UserService.GetUser(username);
        if (dbUser == null) {
            await UserService.AddNewRoleAsync(role);

            dbUser = await UserService.AddNewUserAsync(GetUserInputModel(userId, username, password, role));

            Assert.NotNull(dbUser);
            Assert.True(!string.IsNullOrEmpty(dbUser.Id));
        }


        return new ApplicationUser {
            Id = dbUser.Id.ToString(),
            UserName = dbUser.Name,
            Email = dbUser.Email
        };
    }
    #endregion

    #region AddBearerTokenHeader
    /// <summary>
    /// Default user data will be used if parameters emtpy
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    protected async Task AddBearerTokenHeader(string username = null, string password = null)
    {
        var loginModel = new LoginInputModel { UserName = username ?? ADMIN_USER_NAME, Password = password ?? ADMIN_USER_PASSWORD };
        var response = await HttpClient.PostAsync(GetLoginRoute(), new StringContent(GetJsonFromModel(loginModel), Encoding.UTF8, "application/json"));

        var serverResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        Assert.True(response.IsSuccessStatusCode);
        
        var loginResponse = System.Text.Json.JsonSerializer.Deserialize<LoginOutputModel>(
            await response.Content
            .ReadAsStringAsync());

        Assert.NotNull(loginResponse);
        Assert.True(!string.IsNullOrEmpty(loginResponse.Token));

        HttpClient.DefaultRequestHeaders.Remove("Authorization");

        HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + loginResponse.Token);
    }
    #endregion

    #region AddNewCategoryToDb
    protected async Task<CategoryOutputModel> AddNewCategoryToDb(string name)
    {
        var category = await CategoryRepo.GetCategoryByNameAsync(name);

        if (category == null) {
            var inCategory = new CategoryInputModel {
                Name = name
            };
            return await CategoryRepo.AddCategoryAsync(inCategory);
        }

        return category;
        
    }
    #endregion

    #region AddNewExpenseToDb
    protected async Task<ExpenseOutputModel> AddNewExpenseToDb(Guid userId, Guid categoryId, Guid? expenseId = null, string title = "test", decimal amount = 1, DateTime? date = null)
    {
        ExpenseOutputModel outModel = null;

        if (expenseId.HasValue) {
            outModel = await ExpenseRepo.GetExpenseAsync(userId.ToString(), expenseId.Value);
        }

        if (outModel == null) {
            var model = new ExpenseInputModel {
                Id = expenseId ?? Guid.NewGuid(),
                Title = title,
                Amount = amount,
                CategoryId = categoryId,
                Date = date ?? DateTime.UtcNow
            };

            outModel = await ExpenseRepo.AddNewAsync(userId.ToString(), model);
        }


        return outModel;
    }
    #endregion

    #region ClearDbData
    protected async Task ClearDbData()
    {
        TheDataContext.Expenses.RemoveRange(TheDataContext.Expenses);

        TheDataContext.Categories.RemoveRange(TheDataContext.Categories);

        TheDataContext.Suppliers.RemoveRange(TheDataContext.Suppliers);

        TheDataContext.UserRoles.RemoveRange(TheDataContext.UserRoles);
        TheDataContext.Roles.RemoveRange(TheDataContext.Roles);

        TheDataContext.Users.RemoveRange(TheDataContext.Users);

        await TheDataContext.SaveChangesAsync()
            .ConfigureAwait(false);
    }
    #endregion

    #region GetErrorResponseModel
    protected ITryExpenseTracker.Core.OutputModels.ErrorDetailsOutputModel GetErrorResponseModel(string model)
    {
        return System.Text.Json.JsonSerializer.Deserialize<ITryExpenseTracker.Core.OutputModels.ErrorDetailsOutputModel>(model);            
    }
    #endregion

    #region _checkUserRole
    private void _checkUserRole(string role) {
        if (!string.IsNullOrEmpty(role)) {
            if (!ITryExpenseTracker.Core.Constants.UserRoles.ALL_ROLES.Contains(role)) {
                throw new Exception("Invalid role supplied: " + role);
            }
        }
    }
    #endregion
}
