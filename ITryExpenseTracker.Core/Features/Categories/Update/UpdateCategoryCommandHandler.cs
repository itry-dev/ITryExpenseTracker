using FluentValidation;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Exceptions;
using ITryExpenseTracker.Core.OutputModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Categories.Update;
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryOutputModel> {

    readonly ICategoryRepo _categoryRepo;
    readonly IValidator<UpdateCategoryCommand> _validator;

    public UpdateCategoryCommandHandler(ICategoryRepo repo, IValidator<UpdateCategoryCommand> validator) {
        _categoryRepo = repo;
        _validator = validator;
    }

    public async Task<CategoryOutputModel> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken) {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid) {
            throw new CategoryException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try {
            return await _categoryRepo.UpdateCategoryAsync(request.CategoryInputModel)
                            .ConfigureAwait(false);

        }
        catch (Exception e) {
            throw new CategoryException(e, $"Category cannot be updated");
        }
    }
}
