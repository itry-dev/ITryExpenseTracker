using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Authentication.Abstractions.Abstractions;
using ITryExpenseTracker.Infrastructure;
using ITryExpenseTracker.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ITryExpenseTracker.Migrations;

[ApiController]
[Route("private/api/v1/migrations")]
[Route("api/v1/migrations")]
public class MigrationController : ControllerBase
{
    readonly DataContext _db;
    const string MIGRATIONS_AUTH_KEY = "mkey";
    const string MIGRATIONS_AUTH_VALUE = "2023_ikdle&*[]fdsd,";
    readonly IUserService _userService;
    readonly ICategoryRepo _categoryRepo;

    public MigrationController(DataContext db, IUserService userService, ICategoryRepo categoryRepo)
    {
        _db = db;
        _userService = userService;
        _categoryRepo = categoryRepo;
    }

    [HttpPost]
    public async Task<IActionResult> RunMigrations(SeedDataInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!Request.Headers.Any(a => a.Key == MIGRATIONS_AUTH_KEY)
            || Request.Headers[MIGRATIONS_AUTH_KEY] != MIGRATIONS_AUTH_VALUE)
        {
            return Unauthorized();
        }

        var migrationResult = new MigrationResult();

        var pendingMigrations = await _db.Database.GetPendingMigrationsAsync()
                                .ConfigureAwait(false);

        if (pendingMigrations.Any())
        {
            try
            {
                await _db.Database.MigrateAsync()
                        .ConfigureAwait(false);

                migrationResult.AppliedMigrations = pendingMigrations.ToArray();
            }
            catch (Exception e)
            {
                return BadRequest(new ClientErrorData { Title = e.Message });
            }
        }

        if (!model.SeedData)
        {
            return Ok(migrationResult);
        }

        #region adding admin user
        //adding admin user
        try
        {
            await _userService.AddNewRoleAsync(Core.Constants.UserRoles.ADMIN);
            migrationResult.RoleCreatedResult = "OK";

            try
            {
                await _userService.AddNewUserAsync(new Core.Authentication.Abstractions.InputModels.UserInputModel
                {
                    Email = model.AdminEmail,
                    Name = model.AdminName,
                    UserName = model.AdminUsername,
                    UserRole = Core.Constants.UserRoles.ADMIN,
                    Password = model.AdminPassword,
                    EmailConfirmed = true
                });

                migrationResult.UserCreatedResult = "OK";
            }
            catch (Exception e)
            {
                migrationResult.UserCreatedResult = "ERROR " + e.Message;
            }
        }
        catch (Exception e)
        {
            migrationResult.RoleCreatedResult = "ERROR " + e.Message;
        }
        #endregion

        #region adding categories
        foreach (var category in model.Categories)
        {
            try
            {
                await _categoryRepo.AddCategoryAsync(new Core.InputModels.CategoryInputModel { Name = category }, throwIfExists: false)
                            .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                migrationResult.CategoriesCreatedResult.Add(e.Message);
            }

        }
        #endregion


        return Ok(migrationResult);
    }

    public class MigrationResult
    {
        public string[] AppliedMigrations { get; set; }
        public string RoleCreatedResult { get; set; }
        public string UserCreatedResult { get; set; }

        public List<string> CategoriesCreatedResult { get; set; } = new List<string>();
    }
}