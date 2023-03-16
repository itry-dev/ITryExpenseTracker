using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.OutputModels;

public class ExpenseOutputModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Notes { get; set; } = null;

    public DateTime CreatedAt { get; set; }

    public DateTime Updated { get; set; }

    public CategoryOutputModel Category { get; set; } = null;

    public ExpenseOutputModel Supplier { get; set; } = null;

    public RecurringExpenseOutputModel RecurringExpense { get; set; } = null;    
}

