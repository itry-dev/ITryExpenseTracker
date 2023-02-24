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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.Linq;
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

    protected readonly Guid CATEGORY_GENERAL_ID = Guid.Parse("e1a4b32f-cf30-44b6-bce4-40deb6e37cf7");

    protected IExpenseRepo ExpRepo;

    public BaseIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        Factory = factory;
        HttpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        ExpRepo = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<IExpenseRepo>();

        _seedDb().GetAwaiter().GetResult();
    }

    #region _seedDb
    private async Task _seedDb()
    {
        await AddUserWithRole(ADMIN_ID, ADMIN_USER_NAME, ADMIN_USER_PASSWORD, ITryExpenseTracker.Core.Constants.UserRoles.ADMIN);

        await AddNewCategoryToDb(CATEGORY_GENERAL_ID, "General");
    }
    #endregion

    #region AddUserWithRole
    protected async Task<ApplicationUser> AddUserWithRole(Guid userId, string username, string password,string role)
    {
        using (var scope = Factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var userService = scopedServices.GetRequiredService<IUserService>();

            await userService.AddNewRoleAsync(ITryExpenseTracker.Core.Constants.UserRoles.ADMIN);

            var response = await userService.AddNewUserAsync(GetUserInputModel(userId, username, password, ITryExpenseTracker.Core.Constants.UserRoles.ADMIN));

            return new ApplicationUser
            {
                Id = response.Id.ToString(),
                UserName = response.Name,
                Email = response.Email
            };
        }
    }
    #endregion

    #region GetDatabase
    protected DataContext GetDatabase()
    {
        return Factory.Services.GetRequiredService<DataContext>();
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

        var loginResponse = System.Text.Json.JsonSerializer.Deserialize<LoginOutputModel>(
            await response.Content
            .ReadAsStringAsync());

        Assert.NotNull(loginResponse);
        Assert.True(!string.IsNullOrEmpty(loginResponse.Token));

        HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + loginResponse.Token);
    }
    #endregion

    #region AddNewCategoryToDb
    protected async Task<Category> AddNewCategoryToDb(Guid categoryId, string name)
    {
        using (var scope = Factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();

            var category = new Infrastructure.Models.Category
            {
                Id = categoryId,
                Name = name
            };

            db.Categories.Add(category);

            await db.SaveChangesAsync()
                .ConfigureAwait(false);

            return category;
        }
    }
    #endregion

    #region AddNewExpenseToDb
    protected async Task<ExpenseOutputModel> AddNewExpenseToDb(Guid userId, Guid? expenseId = null, string title = "test", decimal amount = 1, Guid? categoryId = null, DateTime? date = null)
    {

        var model = new ExpenseInputModel
        {
            Id = expenseId ?? Guid.NewGuid(),
            Title = title,
            Amount = amount,
            CategoryId = categoryId ?? CATEGORY_GENERAL_ID,
            Date = date ?? DateTime.UtcNow            
        };

        return await ExpRepo.AddNewAsync(userId.ToString(), model);
    }
    #endregion

    #region ClearDbData
    protected async Task ClearDbData()
    {
        using (var scope = Factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();

            db.Expenses.RemoveRange(db.Expenses.ToList());

            db.Categories.RemoveRange(db.Categories.ToList());

            db.UserRoles.RemoveRange(db.UserRoles.ToList());
            db.Roles.RemoveRange(db.Roles.ToList());

            db.Users.RemoveRange(db.Users.ToList());

            await db.SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
    #endregion

    #region GetErrorResponseModel
    protected ITryExpenseTracker.Core.OutputModels.ErrorDetailsOutputModel GetErrorResponseModel(string model)
    {
        return System.Text.Json.JsonSerializer.Deserialize<ITryExpenseTracker.Core.OutputModels.ErrorDetailsOutputModel>(model);            
    }
    #endregion
}
