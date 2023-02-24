using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Infrastructure.Models;

namespace ITryExpenseTracker.Infrastructure;

public class DataContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, string>
{
    private const string SYSTEM = "system";

    #region GUIDs
    private const string ADMIN_ID = "7116d43b-f829-4b00-aa3e-07a34c7ddc30";
    private const string ROLE_ID = "63974edc-be06-41cf-94da-f003b727fc3a";

    private const string C_GENERAL_ID = "b9e99274-7897-4605-b4fd-038e456f6cec";
    private const string C_BILLS_ID = "dba8a7da-19c2-4260-a5f8-141d7a8c114e";
    private const string C_FOOD_ID = "adf8fe60-e5c4-4d19-9e54-a765d7c7b50c";
    private const string C_LEISURE_ID = "e8e61386-aa9c-427f-877e-7bd13671509f";
    private const string C_INSURENCE_ID = "9f9e0a60-fed8-46a0-9753-86fdc416167a";
    private const string C_MORTGAGE_ID = "0060a8a6-6c5e-42a3-b02f-988d724bf321";
    #endregion

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }


    public DbSet<Expense> Expenses { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(e =>
        {
            e.Property(p => p.Email).IsRequired();
            e.HasIndex(i => i.Email).IsUnique();
        });

        modelBuilder.Entity<Expense>().Property(p => p.Amount).HasPrecision(18, 2);
    }
}
