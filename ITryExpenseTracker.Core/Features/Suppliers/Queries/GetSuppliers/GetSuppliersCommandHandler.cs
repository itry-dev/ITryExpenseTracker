using FluentValidation;
using ITryExpenseTracker.Core.Abstractions;
using ITryExpenseTracker.Core.Features.Expenses;
using ITryExpenseTracker.Core.OutputModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Suppliers.Queries.GetSuppliers;

public class GetSuppliersCommandHandler : IRequestHandler<GetSuppliersCommand, SupplierOutputQueryModel> {

    readonly ISupplierRepo _supplierRepo;
    readonly IValidator<GetSuppliersCommand> _validator;

    public GetSuppliersCommandHandler(ISupplierRepo supplierRepo, IValidator<GetSuppliersCommand> validator) {
        _supplierRepo = supplierRepo;
        _validator = validator;
    }

    public async Task<SupplierOutputQueryModel> Handle(GetSuppliersCommand request, CancellationToken cancellationToken) {
        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
        if (!result.IsValid) {
            throw new SupplierModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try {
            return await _supplierRepo.GetSuppliersAsync(request.UserId, request.DataFilter)
                            .ConfigureAwait(false);
        }
        catch (Exception e) {
            throw new GetSupplierException(e, "Cannot query suppliers");
        }
    }
}
