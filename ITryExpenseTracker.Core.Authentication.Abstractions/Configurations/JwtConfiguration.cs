using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Authentication.Abstractions.Configurations;

public class JwtConfiguration
{
    public string Key { get; set; } = "";

    public string Issuer { get; set; } = "";

    public string Audience { get; set; } = "";

    public int TokenExpiresInMinutes { get; set; }
}
