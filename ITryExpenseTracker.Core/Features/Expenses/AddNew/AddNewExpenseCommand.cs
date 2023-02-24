using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.AddNew;

public class AddNewExpenseCommand : IRequest<ExpenseOutputModel>
{
    public readonly ExpenseInputModel InputModel;
    public readonly string UserId;

    public AddNewExpenseCommand(string userId, ExpenseInputModel model)
    {
        UserId = userId;
        InputModel = model;
    }
}

public sealed class AddNewExpenseCommandValidator : AbstractValidator<AddNewExpenseCommand>
{
    public AddNewExpenseCommandValidator()
    {
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