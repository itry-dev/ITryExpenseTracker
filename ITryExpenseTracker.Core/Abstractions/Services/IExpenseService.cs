using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Abstractions.Services;

public interface IExpenseService
{
    /// <summary>
    /// Scan all the recurring expenses of the last month and copy them to the current month it it's time.
    /// </summary>
    /// <param name="fromDate">a date to start scanning from</param>
    Task ScanAndCopyRecurringExpenses(DateTime? fromDate = null);
}
