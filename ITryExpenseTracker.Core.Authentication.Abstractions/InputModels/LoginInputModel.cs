using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Authentication.Abstractions.InputModels;

public class LoginInputModel
{
    public string UserName { get; set; } = "";

    public string Password { get; set; } = "";
}
