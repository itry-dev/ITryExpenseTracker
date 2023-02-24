using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ITryExpenseTracker.Infrastructure.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(200)]
    public string Name { get; set; }
}
