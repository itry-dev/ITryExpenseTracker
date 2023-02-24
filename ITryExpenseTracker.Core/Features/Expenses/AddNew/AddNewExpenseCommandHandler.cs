using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.AddNew;

public class AddNewExpenseCommandHandler : IRequestHandler<AddNewExpenseCommand, ExpenseOutputModel>
{
    readonly IExpenseRepo _repo;
    readonly IValidator<AddNewExpenseCommand> _validator;

    public AddNewExpenseCommandHandler(IExpenseRepo repo, IValidator<AddNewExpenseCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<ExpenseOutputModel> Handle(AddNewExpenseCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid)
        {
            throw new ExpenseModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try
        {
            return await _repo.AddNewAsync(request.UserId,request.InputModel)
                            .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new AddNewExpenseException(e, $"Expense cannot be added");
        }
    }
}
