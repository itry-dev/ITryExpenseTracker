using ITryExpenseTracker.User.Abstractions.InputModels;
using ITryExpenseTracker.User.Abstractions.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.User.Abstractions.Services;
public interface IUserService {

    Task ChangePassword(string userId, ChangePasswordInputModel model);

    Task SendChangePasswordEmailLink(string userName, string email);

    Task<UserOutputModel?> GetUser(string userId, bool throwIfNotFound = false);

    string GeneratePassword(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial,
        int passwordSize);
}

