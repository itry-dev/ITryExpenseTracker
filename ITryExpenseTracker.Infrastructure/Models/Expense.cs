using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Infrastructure.Abstractions;

namespace ITryExpenseTracker.Infrastructure.Models;

public class Expense : BaseEntity, IUserOwned
{
    [MaxLength(255)]
    public string ApplicationUserId { get; set; }

    [MaxLength(100)]
    public string Title { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public Guid CategoryId { get; set; }

    public Guid? SupplierId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public ApplicationUser ApplicationUser { get; set; }    

    public Category Category { get; set; }

    public Supplier? Supplier { get; set; }

}
