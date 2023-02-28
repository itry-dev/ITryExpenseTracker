using FluentValidation;
using ITryExpenseTracker.Core.Features.Expenses.Queries.GetExpenses;
using ITryExpenseTracker.Core.InputModels;
using ITryExpenseTracker.Core.OutputModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Suppliers.Queries.GetSuppliers;

public class GetSuppliersCommand : IRequest<SupplierOutputQueryModel> {
    public readonly DataFilter DataFilter;
    public readonly string UserId;

    public GetSuppliersCommand(string userId, DataFilter filter) {
        UserId = userId;
        DataFilter = filter;
    }
}

public sealed class GetSuppliersCommandValidator : AbstractValidator<GetSuppliersCommand> {
    public GetSuppliersCommandValidator() {
        RuleFor(r => r.UserId).NotNull();

        When(w => !string.IsNullOrEmpty(w.DataFilter.Q), () => {
            RuleFor(x => x.DataFilter.Q)
            .MinimumLength(2);
        });

    }
}