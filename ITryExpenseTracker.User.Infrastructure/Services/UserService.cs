using ITryExpenseTracker.Infrastructure;
using ITryExpenseTracker.Infrastructure.Models;
using ITryExpenseTracker.User.Abstractions.Exceptions;
using ITryExpenseTracker.User.Abstractions.InputModels;
using ITryExpenseTracker.User.Abstractions.OutputModels;
using ITryExpenseTracker.User.Abstractions.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ITryExpenseTracker.User.Infrastructure.Services;

public class UserService : IUserService
{
    readonly DataContext _dataContext;
    readonly ILogger<UserService> _logger;
    readonly UserManager<ApplicationUser> _userManager;
    readonly IEmailService _emailService;

    const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
    const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const string NUMBERS = "123456789";
    const string SPECIALS = @"!@£$%^&*()#€";

    public UserService(DataContext dataContext, 
                        ILoggerFactory loggerFactory, 
                        UserManager<ApplicationUser> userManager,
                        IEmailService emailService) {
        _dataContext = dataContext;
        _logger = loggerFactory.CreateLogger<UserService>();
        _userManager = userManager;
        _emailService = emailService;
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

    #region SendChangePasswordEmailLink

    public async Task SendChangePasswordEmailLink(string userName, string email) {
        
        var user = await _userManager.FindByNameAsync(userName)
                .ConfigureAwait(false);

        if (user == null) {
            throw new UserNotFoundException(userName);
        }

        var pwd = GeneratePassword(true, true, true, true, 10);
        user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, pwd);

        await _userManager.UpdateAsync(user);

        await _emailService.SendNewPassword(toMail: user.Email, userName: user.UserName, password: pwd)
                .ConfigureAwait(false);
    }
    #endregion

    #region GetUser
    public async Task<UserOutputModel?> GetUser(string userId, bool throwIfNotFound = false) {
        var user = await _userManager.FindByIdAsync(userId)
                .ConfigureAwait(false);

        if (user == null) {
            if (throwIfNotFound) throw new UserNotFoundException(userId);

            return null;
        }

        return new UserOutputModel {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email
        };
    }
    #endregion

    #region GeneratePassword
    public string GeneratePassword(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial,
        int passwordSize) {
        char[] _password = new char[passwordSize];
        string charSet = ""; // Initialise to blank
        System.Random _random = new Random();
        int counter;

        // Build up the character set to choose from
        if (useLowercase) charSet += LOWER_CASE;

        if (useUppercase) charSet += UPPER_CAES;

        if (useNumbers) charSet += NUMBERS;

        if (useSpecial) charSet += SPECIALS;

        for (counter = 0; counter < passwordSize; counter++) {
            _password[counter] = charSet[_random.Next(charSet.Length - 1)];
        }

        return String.Join(null, _password);
    }
    #endregion
}
