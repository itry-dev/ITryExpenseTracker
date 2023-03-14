using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Authentication.Abstractions.Abstractions;
using ITryExpenseTracker.Core.Authentication.Abstractions.InputModels;
using ITryExpenseTracker.Infrastructure;

namespace ITryExpenseTracker.Core.Authentication;

[ApiController]
[Route("private/api/v1/login")]
[Route("api/v1/login")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
        if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
        {
            return BadRequest(new ClientErrorData { Title = "Invalid login" });
        }

        var userModel = await _userService.Login(new LoginInputModel
        {
            UserName = model.UserName,
            Password = model.Password
        }
        );

        return Ok(userModel);
    }

}
