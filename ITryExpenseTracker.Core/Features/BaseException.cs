using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Exceptions;


public abstract class BaseException : Exception
{
    public HttpStatusCode StatusCode { get; private set; }
    public BaseException(HttpStatusCode statusCode, string message, Exception ex = null) : base(message, ex)
    {
        StatusCode = statusCode;
    }
}
