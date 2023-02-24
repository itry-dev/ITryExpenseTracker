using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpense;

public class GetExpenseCommandHandler : IRequestHandler<GetExpenseCommand, ExpenseOutputModel?>
{
    readonly IExpenseRepo _repo;
    readonly IValidator<GetExpenseCommand> _validator;

    public GetExpenseCommandHandler(IExpenseRepo repo, IValidator<GetExpenseCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<ExpenseOutputModel?> Handle(GetExpenseCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid)
        {
            throw new ExpenseModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try
        {
            return await _repo.GetExpenseAsync(request.UserId, request.Id)
                            .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new GetExpenseException(e, $"Cannot get as expense with id {request.Id} for user {request.UserId}");
        }
    }
}
