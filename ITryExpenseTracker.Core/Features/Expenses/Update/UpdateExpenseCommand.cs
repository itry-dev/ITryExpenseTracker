using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.Update;

public class UpdateExpenseCommand : IRequest<ExpenseOutputModel?>
{
    public readonly ExpenseInputModel InputModel;
    public readonly string UserId;

    public UpdateExpenseCommand(string userId, ExpenseInputModel model)
    {
        UserId = userId;
        InputModel = model;
    }
}

public sealed class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(x => x.InputModel.Id)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.InputModel.CategoryId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.InputModel.Title).NotEmpty();

        RuleFor(x => x.InputModel.Amount).NotEmpty();

        When(w => w.InputModel.SupplierId.HasValue, () => 
        {
            RuleFor(x => x.InputModel.SupplierId).NotEqual(Guid.Empty);
        });
    }
}