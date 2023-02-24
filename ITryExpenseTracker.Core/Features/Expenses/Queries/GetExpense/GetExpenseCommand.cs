using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpense;

public class GetExpenseCommand : IRequest<ExpenseOutputModel?>
{
    public readonly Guid Id;
    public readonly string UserId;

    public GetExpenseCommand(string userId, Guid id)
    {
        UserId = userId;
        Id = id;
    }
}

public sealed class GetExpenseCommandValidator : AbstractValidator<GetExpenseCommand>
{
    public GetExpenseCommandValidator()
    {
        RuleFor(r => r.UserId).NotNull();
        RuleFor(r => r.Id).NotNull();
    }
}