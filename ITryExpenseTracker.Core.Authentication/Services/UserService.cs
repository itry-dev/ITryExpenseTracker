using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ITryExpenseTracker.Core.Authentication.Abstractions;
using ITryExpenseTracker.Core.Authentication.Abstractions.Abstractions;
using ITryExpenseTracker.Core.Authentication.Abstractions.Configurations;
using ITryExpenseTracker.Core.Authentication.Abstractions.Exceptions;
using ITryExpenseTracker.Core.Authentication.Abstractions.InputModels;
using ITryExpenseTracker.Core.Authentication.Abstractions.OutputModels;
using ITryExpenseTracker.Infrastructure;
using ITryExpenseTracker.Infrastructure.Models;
using ITryExpenseTracker.Core.Exceptions;

namespace ITryExpenseTracker.Core.Authentication.Services;

public class UserService : IUserService
{
    private readonly DataContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationUserRole> _roleManager;
    private readonly ILogger _logger;
    private readonly JwtConfiguration _jwtConfiguration;

    public UserService(ILoggerFactory loggerFactory,
                        DataContext dataContext,
                        UserManager<ApplicationUser> userManager,
                        RoleManager<ApplicationUserRole> roleManager,
                        IOptions<JwtConfiguration> options)
    {
        _db = dataContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = loggerFactory.CreateLogger<UserService>();
        _jwtConfiguration = options.Value;
    }

    #region AddNewRoleAsync
    public async Task AddNewRoleAsync(string roleName)
    {
        if (string.IsNullOrEmpty(roleName) || !ITryExpenseTracker.Core.Constants.UserRoles.ALL_ROLES.Contains(roleName))
        {
            throw new InvalidRoleException(roleName);
        }

        if (! await _roleManager.RoleExistsAsync(roleName).ConfigureAwait(false))
        {
            await _roleManager.CreateAsync(new ApplicationUserRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });
        }
        
    }
    #endregion

    #region AddNewUserAsync
    public async Task<NewUserOutputModel> AddNewUserAsync(NewUserInputModel model)
    {
        var exists = await _userManager.Users
                        .Where(w => w.Email == model.Email)
                        .AnyAsync()
                        .ConfigureAwait(false);

        if (exists)
        {
            _logger.LogInformation($"A user with email {model.Email} already exists");
            throw new Exception("Cannot add user");
        }

        var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);

        if (user == null)
        {
            user = new ApplicationUser
            {
                Id = model.UserId != Guid.Empty ? model.UserId.ToString() : Guid.NewGuid().ToString(),
                Email = model.Email,
                Name= model.Name,
                NormalizedEmail = model.Email.ToUpper(),
                EmailConfirmed = model.EmailConfirmed,
                UserName = model.UserName,
                NormalizedUserName = model.UserName.ToUpper(),
            };
            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, model.Password);
            _db.Users.Add(user);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            //ruolo
            await _userManager.AddToRoleAsync(user, model.UserRole);
        }        

        return new NewUserOutputModel
        {
            Id = Guid.Parse(user.Id),
            Email = model.Email,
            Name = model.Name,
        };
    }
    #endregion

    #region Login
    public async Task<LoginOutputModel> Login(LoginInputModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
        if (user == null)
        {
            throw new UserNotFoundException(model.UserName);
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false))
        {
            throw new UserNotAuthorizedException(model.UserName);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>();
        claims.Add(new Claim(ITryExpenseTrackerClaims.UserId, user.Id, ClaimValueTypes.String));
        claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String));
        
        if (roles.Any())
        {
            claims.AddRange(roles.Select(s => new Claim
            (
                ClaimTypes.Role,
                s,
                ClaimValueTypes.String
            ))
                .ToList());
        }
        

        var token = JwtHelper.GetJwtToken(
            username: model.UserName,
            uniqueKey: _jwtConfiguration.Key,
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            TimeSpan.FromMinutes(_jwtConfiguration.TokenExpiresInMinutes),
            additionalClaims: claims.ToArray()
            );

        return new LoginOutputModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = token.ValidTo
        };
    }
    #endregion

}