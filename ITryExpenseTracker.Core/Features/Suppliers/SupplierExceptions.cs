using ITryExpenseTracker.Core.Features.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Suppliers;
public class SupplierModelValidationException : BaseException {
    public SupplierModelValidationException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public SupplierModelValidationException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }
}

public class GetSupplierException : BaseException {
    public GetSupplierException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public GetSupplierException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }

}

public class AddNewSupplierException : BaseException {
    public AddNewSupplierException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public AddNewSupplierException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }
}

public class UpdateSupplierException : BaseException {
    public UpdateSupplierException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public UpdateSupplierException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }
}

public class DeleteSupplierException : BaseException {
    public DeleteSupplierException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public DeleteSupplierException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }
}

