using ITryExpenseTracker.Core.Authentication.Abstractions.OutputModels;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ITryExpenseTracker.Tests.IntegrationTests;

public class CategoriesTest : BaseIntegrationTest {

    private readonly ITestOutputHelper _testOutputHelper;

    public CategoriesTest(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task only_admin_role_can_add_category_test() {

        await _addNonAdminUserAndToken();

        var categoryModel = new CategoryInputModel { Name = "test_category" };

        var response = await HttpClient.PostAsync(GetCategoriesRoute(), new StringContent(GetJsonFromModel(categoryModel), Encoding.UTF8, "application/json"));

        Assert.True(!response.IsSuccessStatusCode, $"Server replied with code {response.StatusCode}");
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Forbidden, $"Server replied with code {response.StatusCode}");
    }

    [Fact]
    public async Task only_admin_role_can_update_category_test() {

        await _addNonAdminUserAndToken();

        var categoryModel = new CategoryInputModel { Name = "test_category_mod" };

        var response = await HttpClient.PutAsync(GetCategoriesRoute()+"/"+Guid.NewGuid(), new StringContent(GetJsonFromModel(categoryModel), Encoding.UTF8, "application/json"));

        Assert.True(!response.IsSuccessStatusCode, $"Server replied with code {response.StatusCode}");
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Forbidden, $"Server replied with code {response.StatusCode}");
    }

    [Fact]
    public async Task only_admin_role_can_delete_category_test() {

        await _addNonAdminUserAndToken();

        var response = await HttpClient.DeleteAsync(GetCategoriesRoute()+"/"+Guid.NewGuid());

        Assert.True(!response.IsSuccessStatusCode, $"Server replied with code {response.StatusCode}");
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Forbidden, $"Server replied with code {response.StatusCode}");
    }

    private async Task _addNonAdminUserAndToken() {
        var user = "noadminTestUser";
        var pwd = "blablaStuff!";
        await AddUserWithRole(userId: Guid.NewGuid(),
                              username: user,
                              password: pwd,
                              role: ITryExpenseTracker.Core.Constants.UserRoles.USER);

        await AddBearerTokenHeader(username: user, password: pwd);
    }
}

