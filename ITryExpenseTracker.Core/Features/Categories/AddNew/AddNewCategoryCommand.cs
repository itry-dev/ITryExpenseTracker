using FluentValidation;
using ITryExpenseTracker.Core.Features.Categories.Delete;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Categories.AddNew;

public class AddNewCategoryCommand : IRequest<CategoryOutputModel> {

    public readonly CategoryInputModel CategoryInputModel;

    public AddNewCategoryCommand(CategoryInputModel model) {
        CategoryInputModel = model;
    }
}

public sealed class AddNewCategoryCommandValidator : AbstractValidator<AddNewCategoryCommand> {
    public AddNewCategoryCommandValidator() {
        RuleFor(x => x.CategoryInputModel.Name)
            .NotNull()
            .NotEmpty();
    }
}
