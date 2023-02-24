using ITryExpenseTracker.Core.Authentication.Abstractions;
using ITryExpenseTracker.Infrastructure.Models;
using ITryExpenseTracker.User.Abstractions.InputModels;
using ITryExpenseTracker.User.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace ITryExpenseTracker.User.Api;

[Route("private/api/v1/users")]
[Route("api/v1/users")]
[Authorize]
public class UserController : ControllerBase {

    readonly IUserService _userService;

    public UserController(IUserService userService) {
        _userService = userService;
    }

    #region _getSessionUserId
    private string? _getSessionUserId() {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ITryExpenseTrackerClaims.UserId));
        if (claim == null) {
            return null;
        }

        return claim.Value;
    }
    #endregion

    [HttpPost]
    [Route("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputModel model) {
        //TODO mediatr

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            await _userService.ChangePassword(_getSessionUserId(), model);
            return Ok();
        }
        catch (Exception e) {

            return BadRequest(e.Message);
        }
    }

}


