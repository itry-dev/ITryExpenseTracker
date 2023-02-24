using ITryExpenseTracker.User.Abstractions.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.User.Abstractions.Services;
public interface IUserService {

    Task ChangePassword(string userId, ChangePasswordInputModel model);
}

