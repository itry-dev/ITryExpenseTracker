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

namespace ITryExpenseTracker.Core.Features.Expenses.Update;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, ExpenseOutputModel?>
{
    readonly IExpenseRepo _repo;
    readonly IMediator _mediator;
    readonly IValidator<UpdateExpenseCommand> _validator;

    public UpdateExpenseCommandHandler(IExpenseRepo repo, IMediator mediator, IValidator<UpdateExpenseCommand> validator)
    {
        _repo = repo;
        _mediator = mediator;
        _validator = validator;
    }

    public async Task<ExpenseOutputModel?> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid)
        {
            throw new ExpenseModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try
        {
            await _repo.UpdateAsync(request.UserId, request.InputModel)
                            .ConfigureAwait(false);

            return await _repo.GetExpenseAsync(request.UserId, request.InputModel.Id.GetValueOrDefault())
                        .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new UpdateExpenseException(e, $"Expense cannot be updated");
        }
    }
}
