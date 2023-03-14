using FluentValidation;
using ITryExpenseTracker.Core.Features.Categories.Update;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Suppliers.Update;

public class UpdateSupplierCommand : IRequest<Unit> {

    public readonly SupplierInputModel SupplierInputModel;
    public readonly string UserId;

    public UpdateSupplierCommand(string userId, SupplierInputModel supplierInputModel) {
        SupplierInputModel = supplierInputModel;
        UserId = userId;
    }
}

public sealed class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand> {
    public UpdateSupplierCommandValidator() {
        RuleFor(x => x.SupplierInputModel.Id)
            .NotNull()
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.SupplierInputModel.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty();
    }
}
