using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.OutputModels;

public class ExpenseOutputSlimModel
{
    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}
