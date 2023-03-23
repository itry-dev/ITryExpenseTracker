using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Tests.UnitTest;

public class ExpenseServiceTest : BaseUnitTest
{
    #region recurring_expense_added_once_per_month_test
    [Fact]
    public async Task recurring_expense_added_once_per_month_test()
    {
        //arrange
        var expSvc = GetExpenseService();

        //add new recurring expense, date last month
        var expModel = new Core.InputModels.ExpenseInputModel
        {
            Id = Guid.NewGuid(),
            Title = "test",
            Date = DateTime.UtcNow.AddMonths(-1),
            StartDate = DateTime.UtcNow,
            Amount = 1,
            CategoryId = DEFAULT_CATEGORY_ID
        };

        var expRepo = await GetExpenseRepo()
                        .AddNewAsync(ADMIN_ID, expModel)
                        .ConfigureAwait(false);

        //expSvc should add new recurring expense
        await expSvc.ScanAndCopyRecurringExpenses();

        var numOcc = await DbContext.Expenses.Where(
                            w => w.Date.Month == expModel.Date.Value.AddMonths(1).Month
                            && w.Title == expModel.Title
                            && w.Amount == expModel.Amount)
                            .CountAsync()
                            .ConfigureAwait(false);

        //expSvc should not add the already added recurring expense
        await expSvc.ScanAndCopyRecurringExpenses();

        var numOcc2 = await DbContext.Expenses.Where(
                            w => w.Date.Month == expModel.Date.Value.AddMonths(1).Month
                            && w.Title == expModel.Title
                            && w.Amount == expModel.Amount)
                            .CountAsync()
                            .ConfigureAwait(false);



        Assert.True(numOcc == 1);
        Assert.True(numOcc2 == 1);

    } 
    #endregion
}
