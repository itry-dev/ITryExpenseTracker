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

namespace ITryExpenseTracker.Core.Features.Suppliers.Update {

    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, Unit> {

        readonly ISupplierRepo _repo;
        readonly IValidator<UpdateSupplierCommand> _validator;

        public UpdateSupplierCommandHandler(ISupplierRepo repo, IValidator<UpdateSupplierCommand> validator) {
            _repo = repo;
            _validator = validator;
        }

        public async Task<Unit> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken) {
            var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
            if (!result.IsValid) {
                throw new SupplierModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
            }

            try {
                await _repo.UpdateAsync(request.UserId, request.SupplierInputModel)
                                .ConfigureAwait(false);

                return Unit.Value;

            }
            catch (Exception e) {
                throw new UpdateSupplierException(e, $"Supplier cannot be updated");
            }
        }
    }
}
