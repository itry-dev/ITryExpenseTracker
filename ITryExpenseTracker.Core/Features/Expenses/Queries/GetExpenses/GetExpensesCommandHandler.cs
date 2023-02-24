using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpenses;

public class GetExpensesCommandHandler : IRequestHandler<GetExpensesCommand, ExpenseOutputQueryModel>
{
    readonly IExpenseRepo _repo;
    readonly IValidator<GetExpensesCommand> _validator;

    public GetExpensesCommandHandler(IExpenseRepo repo, IValidator<GetExpensesCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<ExpenseOutputQueryModel> Handle(GetExpensesCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid)
        {
            throw new ExpenseModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try
        {
            return await _repo.GetExpensesAsync(request.UserId, request.DataFilter)
                            .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new GetExpenseException(e, "Cannot query expenses");
        }
    }
}
