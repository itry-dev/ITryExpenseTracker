using FluentValidation;
using ITryExpenseTracker.Core.Features.Suppliers.Update;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Suppliers.Delete;

public class DeleteSupplierCommand : IRequest<Unit> {

    public readonly Guid SupplierId;
    public readonly string UserId;

    public DeleteSupplierCommand(string userId, Guid supplierId) {
        SupplierId = supplierId;
        UserId = userId;
    }
}

public sealed class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand> {
    public DeleteSupplierCommandValidator() {
        RuleFor(x => x.SupplierId)
            .NotNull()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty();
    }
}
