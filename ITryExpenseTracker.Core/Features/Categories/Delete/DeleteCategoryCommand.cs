using FluentValidation;
using ITryExpenseTracker.Core.Features.Expenses.AddNew;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Categories.Delete;

public class DeleteCategoryCommand : IRequest<Unit>
{
    public readonly Guid CategoryId;

    public DeleteCategoryCommand(Guid id)
    {
        CategoryId = id;
    }
}

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
