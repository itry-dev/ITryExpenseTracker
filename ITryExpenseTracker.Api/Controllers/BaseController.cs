using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ITryExpenseTracker.Core.Authentication.Abstractions;
using ITryExpenseTracker.Core.Features.Expenses.Update;
using ITryExpenseTracker.Core.InputModels;
using Microsoft.AspNetCore.Authorization;

namespace ITryExpenseTracker.Api.Controllers;

public class BaseController : ControllerBase
{
    #region GetSessionUserId
    /// <summary>
    /// The user id or null if not found
    /// </summary>
    /// <returns></returns>
    protected string? GetSessionUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ITryExpenseTrackerClaims.UserId));
        if (claim == null)
        {
            return null;
        }

        return claim.Value;
    }
    #endregion

}
