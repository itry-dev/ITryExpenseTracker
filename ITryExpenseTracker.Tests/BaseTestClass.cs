using AutoMapper;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Core.Authentication.Abstractions.Configurations;
using ITryExpenseTracker.Core.Authentication.Abstractions.InputModels;
using ITryExpenseTracker.Infrastructure;
using ITryExpenseTracker.Infrastructure.Repositories;
using ITryExpenseTracker.Mapper.Services;
using ITryExpenseTracker.Scanner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace ITryExpenseTracker.Tests;

public class BaseTestClass
{
    private IExpenseMapperService _expenseMapperService;

    private ICategoryMapperService _categoryMapperService;

    private IMapper _autoMapper;

    private IOptions<JwtConfiguration> _jwtConfiguration;
  
    protected ILoggerFactory LoggerFactory = new NullLoggerFactory();
    
    #region GetUserInputModel
    protected UserInputModel GetUserInputModel(Guid userId, string username, string password, string role, string email = null)
    {
        return new UserInputModel
        {
            UserId = userId,
            Email = email ?? Guid.NewGuid().ToString().Replace("-","")+"@bar",
            EmailConfirmed = true,
            Name = username,
            UserName = username,
            Password = password,
            UserRole = role
        };
    }
    #endregion

    #region GetLoginRoute
    protected string GetLoginRoute()
    {
        return "/private/api/v1/login";
    }
    #endregion

    #region GetCategoriesRoute
    protected string GetCategoriesRoute() {
        return "/private/api/v1/categories";
    }
    #endregion

    #region GetSuppliersRoute
    protected string GetSuppliersRoute() {
        return "/private/api/v1/suppliers";
    }
    #endregion

    #region GetRecurringExpenseMainRoute
    protected string GetRecurringExpensesMainRoute()
    {
        return "/private/api/v1/recurringexpenses";
    }
    #endregion

    #region GetExpensesMainRoute
    protected string GetExpensesMainRoute()
    {
        return "private/api/v1/expenses";
    } 
    #endregion

    #region GetJsonFromModel
    protected string GetJsonFromModel<T>(T model) where T : class
    {
        return System.Text.Json.JsonSerializer.Serialize(model);
    } 
    #endregion

    #region GetJwtConfigOptions
    protected IOptions<JwtConfiguration> GetJwtConfigOptions()
    {
        if (_jwtConfiguration == null)
        {
            return Options.Create<JwtConfiguration>(new JwtConfiguration
            {
                Key = "dddd",
                TokenExpiresInMinutes = 10,
                Issuer = "foo",
                Audience = "bar"
            });
        }

        return _jwtConfiguration;
    }
    #endregion

    #region GetAutoMapper
    protected IMapper GetAutoMapper()
    {
        if (_autoMapper == null)
        {
            var myProfile = new ITryExpenseTracker.Mapper.MapperProfiles.ProfileManager();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _autoMapper = new AutoMapper.Mapper(configuration);
        }

        return _autoMapper;
    }
    #endregion

    #region GetCategoryMapperService
    protected ICategoryMapperService GetCategoryMapperService()
    {
        if (_categoryMapperService == null)
        {
            _categoryMapperService = new CategoryMapperService(GetAutoMapper());
        }

        return _categoryMapperService;
    }
    #endregion

    #region GetExpenseMapperService
    protected IExpenseMapperService GetExpenseMapperService()
    {
        if (_expenseMapperService == null)
        {
            _expenseMapperService = new ExpenseMapperService(GetAutoMapper());
        }

        return _expenseMapperService;
    }
    #endregion

    
}
