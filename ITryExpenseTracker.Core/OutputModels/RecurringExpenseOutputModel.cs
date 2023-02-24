using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.OutputModels;

public class RecurringExpenseOutputModel
{
    public List<RecurringExpenseOutputModel> Entities { get; set; } = new List<RecurringExpenseOutputModel>();
}
