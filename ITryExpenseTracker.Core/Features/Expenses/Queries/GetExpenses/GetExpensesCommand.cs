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

public class GetExpensesCommand : IRequest<ExpenseOutputQueryModel>
{
    public readonly DataFilter DataFilter;
    public readonly string UserId;

    public GetExpensesCommand(string userId, DataFilter filter)
    {
        UserId = userId;
        DataFilter = filter;
    }
}

public sealed class GetExpensesCommandValidator : AbstractValidator<GetExpensesCommand>
{
    public GetExpensesCommandValidator()
    {
        RuleFor(r => r.UserId).NotNull();

        When(w => !string.IsNullOrEmpty(w.DataFilter.Q), () =>
        {
            RuleFor(x => x.DataFilter.Q)
            .MinimumLength(2);
        });

    }
}