using FluentValidation;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Exceptions;
using ITryExpenseTracker.Core.Features.Expenses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Categories.Delete;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    readonly ICategoryRepo _categoryRepo;
    readonly IValidator<DeleteCategoryCommand> _validator;

    public DeleteCategoryCommandHandler(ICategoryRepo repo, IValidator<DeleteCategoryCommand> validator)
    {
        _categoryRepo = repo;
        _validator = validator;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid)
        {
            throw new CategoryException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try
        {
            await _categoryRepo.DeleteCategoryAsync(request.CategoryId)
                            .ConfigureAwait(false);

            return Unit.Value;
        }
        catch (Exception e)
        {
            throw new CategoryException(e, $"Category cannot be deleted");
        }

        
    }
}
