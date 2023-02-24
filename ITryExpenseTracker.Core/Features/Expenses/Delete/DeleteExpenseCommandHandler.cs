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

namespace ITryExpenseTracker.Core.Features.Expenses.Delete;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, Unit>
{
    readonly IExpenseRepo _repo;
    readonly IValidator<DeleteExpenseCommand> _validator;

    public DeleteExpenseCommandHandler(IExpenseRepo repo, IValidator<DeleteExpenseCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid)
        {
            throw new ExpenseModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try
        {
            await _repo.DeleteAsync(request.UserId, request.ModelId)
                            .ConfigureAwait(false);

            return Unit.Value;
        }
        catch (Exception e)
        {
            throw new DeleteExpenseException(e, $"Expense cannot be deleted");
        }
    }
}
