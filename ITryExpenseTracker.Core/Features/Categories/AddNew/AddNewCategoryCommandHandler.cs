using FluentValidation;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Exceptions;
using ITryExpenseTracker.Core.Features.Categories.Delete;
using ITryExpenseTracker.Core.OutputModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Categories.AddNew;

public class AddNewCategoryCommandHandler : IRequestHandler<AddNewCategoryCommand, CategoryOutputModel> {

    readonly ICategoryRepo _categoryRepo;
    readonly IValidator<AddNewCategoryCommand> _validator;

    public AddNewCategoryCommandHandler(ICategoryRepo repo, IValidator<AddNewCategoryCommand> validator) {
        _categoryRepo = repo;
        _validator = validator;
    }

    public async Task<CategoryOutputModel> Handle(AddNewCategoryCommand request, CancellationToken cancellationToken) {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid) {
            throw new CategoryException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try {
            return await _categoryRepo.AddCategoryAsync(request.CategoryInputModel)
                            .ConfigureAwait(false);

        }
        catch (Exception e) {
            throw new CategoryException(e, $"Category cannot be added");
        }
    }
}
