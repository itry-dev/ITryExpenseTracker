using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.OutputModels;

namespace ITryExpenseTracker.Core.Features.Expenses.Queries.GetSum;

public class GetSumCommandHandler : IRequestHandler<GetSumCommand, ExpenseSumOutputModel>
{
    readonly IExpenseRepo _repo;
    readonly IValidator<GetSumCommand> _validator;

    public GetSumCommandHandler(IExpenseRepo repo, IValidator<GetSumCommand> validator)
    {
        _repo= repo;
        _validator= validator;
    }

    public async Task<ExpenseSumOutputModel> Handle(GetSumCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid)
        {
            throw new ExpenseModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try
        {
            return await _repo.GetSumAsync(request.UserId, request.DataFilter)
                            .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new GetSumException(e, "Cannot query expenses");
        }
    }
}
