using ITryExpenseTracker.Infrastructure;
using ITryExpenseTracker.Infrastructure.Models;
using ITryExpenseTracker.User.Abstractions.Exceptions;
using ITryExpenseTracker.User.Abstractions.InputModels;
using ITryExpenseTracker.User.Abstractions.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ITryExpenseTracker.User.Infrastructure.Services;

public class UserService : IUserService
{
    readonly DataContext _dataContext;
    readonly ILogger<UserService> _logger;
    readonly UserManager<ApplicationUser> _userManager;

    public UserService(DataContext dataContext, ILoggerFactory loggerFactory, UserManager<ApplicationUser> userManager) {
        _dataContext = dataContext;
        _logger = loggerFactory.CreateLogger<UserService>();
        _userManager = userManager;
    }

    #region ChangePassword
    public async Task ChangePassword(string userId, ChangePasswordInputModel model) {
        var user = await _userManager.FindByIdAsync(userId)
                .ConfigureAwait(false);

        if (user == null) {
            throw new UserNotFoundException(userId);
        }

        user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, model.Password);

        await _userManager.UpdateAsync(user)
                .ConfigureAwait(false);
    } 
    #endregion
}