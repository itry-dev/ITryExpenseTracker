using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.Delete;

public class DeleteExpenseCommand : IRequest<Unit>
{
    public readonly Guid ModelId;
    public readonly string UserId;

    public DeleteExpenseCommand(string userId, Guid modelId)
    {
        UserId = userId;
        ModelId = modelId;
    }
}

public sealed class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ModelId)
            .NotEmpty();
    }
}