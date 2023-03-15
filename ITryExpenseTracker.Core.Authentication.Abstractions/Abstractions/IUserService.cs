using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Authentication.Abstractions.InputModels;
using ITryExpenseTracker.Core.Authentication.Abstractions.OutputModels;

namespace ITryExpenseTracker.Core.Authentication.Abstractions.Abstractions;

public interface IUserService
{
    Task<LoginOutputModel> Login(LoginInputModel model);

    Task AddNewRoleAsync(string roleName);

    Task<UserOutputModel> AddNewUserAsync(UserInputModel model);

    Task<UserOutputModel?> GetUser(string userName);  

}
