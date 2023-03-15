using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System;
using ITryExpenseTracker.Core.InputModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using ITryExpenseTracker.Infrastructure.Models;
using ITryExpenseTracker.Core.Authentication.Abstractions.Abstractions;
using ITryExpenseTracker.Core.Authentication.Abstractions.InputModels;
using ITryExpenseTracker.Core.Authentication.Abstractions.OutputModels;
using Xunit.Abstractions;

namespace ITryExpenseTracker.Tests.IntegrationTests;

public class ExpensesTest : BaseIntegrationTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ExpensesTest(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory)
    { 
        _testOutputHelper = testOutputHelper;
    }

    #region cannot_add_expense_categoryid_not_exists_test
    [Fact]
    public async Task cannot_add_expense_categoryid_not_exists_test()
    {
        await AddBearerTokenHeader();

        //arrange
        var model = new ExpenseInputModel
        {
            Id = Guid.NewGuid(),
            Title = "test",
            Amount = 1,
            CategoryId = Guid.NewGuid()
        };

        //act
        var response = await HttpClient.PostAsync(GetExpensesMainRoute(), new StringContent(GetJsonFromModel(model), Encoding.UTF8, "application/json"));

        //assert
        var errorModel = GetErrorResponseModel(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, errorModel.StatusCode);

        _testOutputHelper.WriteLine(errorModel.Message);

        Assert.Equal(typeof(ITryExpenseTracker.Core.Features.Expenses.AddNewExpenseException).FullName, errorModel.ExceptionType);

        Assert.Equal(typeof(ITryExpenseTracker.Core.Exceptions.InvalidCategoryIdException).FullName, errorModel.InnerExceptionType);

    }
    #endregion

    #region cannot_update_expense_categoryid_not_exists_test
    [Fact]
    public async Task cannot_update_expense_categoryid_not_exists_test()
    {
        await AddBearerTokenHeader();

        //arrange
        var expenseId = Guid.NewGuid();

        //add a category
        var category = await AddNewCategoryToDb("test");
        //add a model into db
        await AddNewExpenseToDb(ADMIN_ID, category.Id, expenseId, "test", 1);

        //create an update expense model
        var model = new ExpenseInputModel
        {
            Title = "test mod",
            Amount = 1,
            CategoryId = Guid.NewGuid() //invalid category id
        };

        //act
        var response = await HttpClient.PutAsync(GetExpensesMainRoute() + "/" + expenseId, new StringContent(GetJsonFromModel(model), Encoding.UTF8, "application/json"));

        //assert
        Assert.True(!response.IsSuccessStatusCode);

        var errorModel = GetErrorResponseModel(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        _testOutputHelper.WriteLine(errorModel.Message);

        Assert.Equal(typeof(ITryExpenseTracker.Core.Features.Expenses.UpdateExpenseException).FullName, errorModel.ExceptionType);

        Assert.Equal(typeof(ITryExpenseTracker.Core.Exceptions.InvalidCategoryIdException).FullName, errorModel.InnerExceptionType);
    }
    #endregion

    #region cannot_update_not_owned_expense
    [Fact]
    public async Task cannot_update_not_owned_expense()
    {

        var userId = Guid.NewGuid();

        //add a new user
        await AddUserWithRole(userId, "foo", "foopassword", ITryExpenseTracker.Core.Constants.UserRoles.ADMIN);

        await AddBearerTokenHeader("foo", "foopassword");

        //arrange
        var expenseId = Guid.NewGuid();

        //add a category
        var category = await AddNewCategoryToDb("test");
        //add an expense into db but with admin id owner
        await AddNewExpenseToDb(ADMIN_ID, category.Id, expenseId, "test", 1);

        //try to update an expense not owned by the newly created user
        var model = new ExpenseInputModel
        {
            Title = "test mod",
            Amount = 1,
            CategoryId = category.Id
        };

        //act
        var response = await HttpClient.PutAsync(GetExpensesMainRoute() + "/" + expenseId, new StringContent(GetJsonFromModel(model), Encoding.UTF8, "application/json"));

        //assert
        Assert.True(!response.IsSuccessStatusCode);

        var errorModel = GetErrorResponseModel(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

        _testOutputHelper.WriteLine(errorModel.Message);

        Assert.Equal(typeof(ITryExpenseTracker.Core.Features.Expenses.UpdateExpenseException).FullName, errorModel.ExceptionType);

        //cannot find the expense
        Assert.Equal(typeof(ITryExpenseTracker.Core.Exceptions.ExpenseNotFoundException).FullName, errorModel.InnerExceptionType);

    } 
    #endregion
}
