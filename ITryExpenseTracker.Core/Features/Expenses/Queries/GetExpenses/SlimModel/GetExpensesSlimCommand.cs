using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpenses;

public class GetExpensesSlimCommand : IRequest<ExpenseOutSlimQueryModel>
{
    public readonly DataFilter DataFilter;
    public readonly string UserId;

    public GetExpensesSlimCommand(string userId, DataFilter filter)
    {
        UserId = userId;
        DataFilter = filter;
    }
}

public sealed class GetExpensesSlimCommandValidator : AbstractValidator<GetExpensesSlimCommand>
{
    public GetExpensesSlimCommandValidator()
    {
        RuleFor(r => r.UserId).NotNull();
    }
}