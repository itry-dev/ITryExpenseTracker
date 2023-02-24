using ITryExpenseTracker.Core.Abstractions.Services;
using ITryExpenseTracker.Infrastructure;
using ITryExpenseTracker.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
namespace ITryExpenseTracker.Scanner;

public class ExpenseService : IExpenseService
{
    private readonly ILogger _logger;
    private readonly DataContext _dbContext;

    public ExpenseService(DataContext dbContext, ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _logger = loggerFactory.CreateLogger<ExpenseService>();
    }

    public async Task ScanAndCopyRecurringExpenses(DateTime? fromDate = null)
    {
        var theDate = DateTime.UtcNow;
        if (fromDate.HasValue) { theDate =  fromDate.Value; }

        var expenses = await _dbContext.Expenses.Where(w => w.StartDate.HasValue
                                                && (!w.EndDate.HasValue || w.EndDate.HasValue && w.EndDate >= DateTime.UtcNow) 
                                                && w.Date.Month == theDate.AddMonths(-1).Month)
                        .ToListAsync()
                        .ConfigureAwait(false);

        foreach (var expense in expenses) {
            var oneRow = _dbContext.Expenses.Where(w => w.StartDate.HasValue
                                                   && (!w.EndDate.HasValue 
                                                   || (w.EndDate.HasValue && expense.EndDate.HasValue)
                                                   && w.EndDate.Value.Date >= expense.EndDate.Value.Date)
                                                   && w.Date.Month == theDate.Month
                                                   && w.Amount == expense.Amount
                                                   && w.CategoryId == expense.CategoryId
                                                   && w.ApplicationUserId == expense.ApplicationUserId);

            var num = await oneRow
                        .CountAsync()
                        .ConfigureAwait(false);

            if (num > 1) {
                throw new ScannerExceptions($"Adding new recurring expense cannot be performed, more than one row was found with same data. Number of occurences: {num}");
            }

            if (num==0) {
                var ex = new Expense();
                ex.Id = Guid.NewGuid();
                ex.Amount = expense.Amount;
                ex.ApplicationUserId = expense.ApplicationUserId;
                ex.Notes = expense.Notes;
                ex.Date = theDate;
                ex.CategoryId = expense.CategoryId;
                if (ex.SupplierId.HasValue) ex.SupplierId = expense.SupplierId;
                ex.Title = expense.Title;
                ex.StartDate = expense.StartDate;
                if (expense.EndDate.HasValue) ex.EndDate = expense.EndDate;
                ex.CreatedAt = DateTime.UtcNow;
                ex.CreatedBy = "system";

                _dbContext.Expenses.Add(ex);
                await _dbContext.SaveChangesAsync()
                        .ConfigureAwait(false);
            }
        }
    }
}
