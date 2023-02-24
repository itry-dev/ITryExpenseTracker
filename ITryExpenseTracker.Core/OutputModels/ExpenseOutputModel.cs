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

    public CategoryOutModel Category { get; set; } = null;

    public SupplierOutModel Supplier { get; set; } = null;

    public RecurringExpenseOutputModel RecurringExpense { get; set; } = null;    
}

