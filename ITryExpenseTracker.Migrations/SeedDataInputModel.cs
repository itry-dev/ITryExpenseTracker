using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Migrations;

public class SeedDataInputModel
{
    [Required]
    public bool SeedData { get; set; }

    [Required(ErrorMessage = "Admin name cannot be empty")]
    public string AdminName { get; set; }

    [Required(ErrorMessage = "Admin username cannot be empty")]
    public string AdminUsername { get; set; }

    [Required(ErrorMessage = "Admin password cannot be empty")]
    public string AdminPassword { get; set; }

    [Required(ErrorMessage = "Admin email cannot be empty")]
    public string AdminEmail { get; set; }

    [Required(ErrorMessage = "Categories cannot by empty or null. Plese provide an array of defailt categories names")]
    public string[] Categories { get; set; }

    
}
