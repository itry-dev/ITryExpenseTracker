using MediatR;
using System.Reflection;
using ITryExpenseTracker.Core.Authentication.Configurations.Extensions;
using ITryExpenseTracker.Core.Middlewares;
using ITryExpenseTracker.Infrastructure.Configurations.Extensions;
using ITryExpenseTracker.Mapper.Configurations.Extensions;
using Microsoft.AspNetCore.Authentication;
using ITryExpenseTracker.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ITryExpenseTracker.User.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddRepos();
builder.Services.AddCommandHandlers();
builder.Services.AddModelMappers();

builder.Services.AddUserServices();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy =>
        policy.RequireAssertion(context => 
            context.User.HasClaim(c => c.Type == ClaimTypes.Role 
                                  && c.Value == Constants.UserRoles.ADMIN))
        );
});

builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddCors(config =>
{
    config.AddPolicy("default", configP =>
    {
        configP.AllowAnyHeader();
        configP.WithExposedHeaders("X-Total-Count");

        configP.WithOrigins("http://192.168.1.3:5173", "http://localhost:5173", "http://localhost:4200", "http://expensestracker.local");
        configP.WithMethods("OPTIONS", "PUT", "POST", "GET", "DELETE");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyName: "default");

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

//for unit test
public partial class Program { }