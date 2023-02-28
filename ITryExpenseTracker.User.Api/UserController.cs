using ITryExpenseTracker.Core.Authentication.Abstractions;
using ITryExpenseTracker.Infrastructure.Models;
using ITryExpenseTracker.User.Abstractions.InputModels;
using ITryExpenseTracker.User.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;


namespace ITryExpenseTracker.User.Api;

[Route("private/api/v1/users")]
[Route("api/v1/users")]
[Authorize]
public class UserController : ControllerBase {

    readonly IUserService _userService;
    readonly IEmailService _emailService;
    readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, IEmailService emailService, ILoggerFactory loggerFactory) {
        _userService = userService;
        _emailService = emailService;
        _logger = loggerFactory.CreateLogger<UserController>();
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

    [HttpPost]
    [Route("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordInputModel model) {
        //TODO mediatr

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            await _userService.SendChangePasswordEmailLink(model.Username, model.Email);
            return Ok();
        }
        catch (Exception e) {

            return BadRequest(e.Message);
        }
    }


    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    [HttpGet("testemail")]
    public async Task<IActionResult> testEmail([FromHeader] string testOp) {

        if (!string.IsNullOrEmpty(testOp)) {
            if (testOp == "fdaf5435!,") {
                await _emailService.TestEmail();
                return Ok();
            }
        }

        _logger.LogDebug($"[UserController:testEmail:NoContent response] header key: testOp, header key value: {testOp}");
        return NoContent();
    }

}


