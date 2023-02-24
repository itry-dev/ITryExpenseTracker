using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Authentication.Abstractions.Abstractions;
using ITryExpenseTracker.Core.Authentication.Abstractions.Configurations;
using ITryExpenseTracker.Core.Authentication.Services;

namespace ITryExpenseTracker.Core.Authentication.Configurations.Extensions;

public static class AddAuthenticationServiceExtensions
{
    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var root = "Jwt";
            var issuer = "";
            var audience = "";

            if (string.IsNullOrEmpty(configuration[$"{root}:Issuer"])
                || string.IsNullOrEmpty(configuration[$"{root}:Audience"]))
            {
                throw new ArgumentException($"{root} section has null or empty Issuer and/or null or empty Audience");
            }

            issuer = configuration[$"{root}:Issuer"];
            audience = configuration[$"{root}:Audience"];

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new
                SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[$"{root}:Key"])),
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddTransient<IUserService, UserService>();

        services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));
    }
}
