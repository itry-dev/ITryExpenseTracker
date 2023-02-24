using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Authentication.Abstractions.OutputModels;

public class LoginOutputModel
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; } = "";

    [JsonPropertyName("expires_in")]
    public DateTime Expires { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; } = "Bearer";
}
