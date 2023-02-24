using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Infrastructure.Abstractions;

namespace ITryExpenseTracker.Infrastructure.Models;

public class Supplier : BaseEntity
{
    [MaxLength(200)]
    public string Name { get; set; }


}
