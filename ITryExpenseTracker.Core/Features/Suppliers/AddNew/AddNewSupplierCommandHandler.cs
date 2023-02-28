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

namespace ITryExpenseTracker.Core.Features.Suppliers.AddNew;

public class AddNewSupplierCommandHandler : IRequestHandler<AddNewSupplierCommand, SupplierOutputModel> {

    readonly ISupplierRepo _repo;
    readonly IValidator<AddNewSupplierCommand> _validator;

    public AddNewSupplierCommandHandler(ISupplierRepo repo, IValidator<AddNewSupplierCommand> validator) {
        _repo = repo;
        _validator = validator;
    }

    public async Task<SupplierOutputModel> Handle(AddNewSupplierCommand request, CancellationToken cancellationToken) {

        var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);

        if (!result.IsValid) {
            throw new SupplierModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
        }

        try {
            return await _repo.AddNewAsync(request.UserId, request.InputModel)
                            .ConfigureAwait(false);
        }
        catch (Exception e) {
            throw new AddNewSupplierException(e, $"Supplier cannot be added");
        }
    }
}
