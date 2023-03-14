using FluentValidation;
using ITryExpenseTracker.Core.Features.Categories.AddNew;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Categories.Update;
public class UpdateCategoryCommand : IRequest<CategoryOutputModel> {

    public readonly CategoryInputModel CategoryInputModel;

    public UpdateCategoryCommand(CategoryInputModel model) {
        CategoryInputModel = model;
    }
}

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand> {
    public UpdateCategoryCommandValidator() {
        RuleFor(x => x.CategoryInputModel.Id)
            .NotNull()
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.CategoryInputModel.Name)
            .NotNull()
            .NotEmpty();
    }
}