using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Authentication.Abstractions.InputModels;

public class NewUserInputModel
{
    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public bool EmailConfirmed { get; set; } = false;

    public string UserName { get; set; } = "";

    public string Password { get; set; } = "";

    public string UserRole { get; set; } = "";

    [JsonIgnore]
    public Guid UserId { get; set; }
}
