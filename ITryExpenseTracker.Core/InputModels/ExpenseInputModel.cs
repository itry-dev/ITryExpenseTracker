using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.InputModels;

public class ExpenseInputModel
{

    public string Title { get; set; }

    public decimal Amount { get; set; }

    public DateTime? Date { get; set; } = DateTime.UtcNow;

    public Guid CategoryId { get; set; }

    public Guid? SupplierId { get; set; }

    public string? Notes { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public Guid? Id { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public Guid? RecurringExpenseId { get; set; }
}
