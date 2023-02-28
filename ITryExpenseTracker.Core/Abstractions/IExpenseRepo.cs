using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Abstractions;

public interface IExpenseRepo
{
    Task<ExpenseOutputQueryModel> GetExpensesAsync(string userId, DataFilter filter);

    Task<ExpenseOutputModel> AddNewAsync(string userId, ExpenseInputModel model);

    Task UpdateAsync(string userId, ExpenseInputModel model);

    Task DeleteAsync(string userId, Guid modelId);
    
    /// <summary>
    /// The expense or null if not found
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ExpenseOutputModel?> GetExpenseAsync(string userId, Guid id);

    /// <summary>
    /// Expenses sum
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="ITryExpenseTracker.Core.Exceptions.ExpenseSumException">If <paramref name="filter"/>Q filter</exception> is valid but no year passed
    Task<ExpenseSumOutputModel> GetSumAsync(string userId, DataFilter filter);

    Task<ExpenseOutputSlimQueryModel> GetExpensesSlimAsync(string userId, DataFilter filter);

}
