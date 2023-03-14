using FluentValidation;
using ITryExpenseTracker.Core.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Suppliers.Delete {
    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, Unit> {

        readonly ISupplierRepo _repo;
        readonly IValidator<DeleteSupplierCommand> _validator;

        public DeleteSupplierCommandHandler(ISupplierRepo repo, IValidator<DeleteSupplierCommand> validator) {
            _repo = repo;
            _validator = validator;
        }

        public async Task<Unit> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken) {
            var result = await _validator.ValidateAsync(request)
                                .ConfigureAwait(false);
            if (!result.IsValid) {
                throw new SupplierModelValidationException(string.Join(",", result.Errors.Select(s => s.ErrorMessage)));
            }

            try {
                await _repo.DeleteAsync(request.SupplierId, request.UserId)
                                .ConfigureAwait(false);

                return Unit.Value;

            }
            catch (Exception e) {
                throw new DeleteSupplierException(e, $"Supplier cannot be deleted");
            }
        }
    }
}
