using ITryExpenseTracker.Core.Features.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Expenses;

public class AddNewExpenseException : BaseException
{
    public AddNewExpenseException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public AddNewExpenseException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }
}

public class DeleteExpenseException : BaseException
{
    public DeleteExpenseException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public DeleteExpenseException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }
}

public class GetExpenseException : BaseException
{
    public GetExpenseException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public GetExpenseException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }

}

public class GetSumException : BaseException
{
    public GetSumException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public GetSumException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }
}

public class UpdateExpenseException : BaseException
{
    public UpdateExpenseException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public UpdateExpenseException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }
}

public class ExpenseModelValidationException : BaseException
{
    public ExpenseModelValidationException(string message) : base(HttpStatusCode.BadRequest, message) { }

    public ExpenseModelValidationException(Exception e, string message) : base(HttpStatusCode.BadRequest, message, e) { }
}