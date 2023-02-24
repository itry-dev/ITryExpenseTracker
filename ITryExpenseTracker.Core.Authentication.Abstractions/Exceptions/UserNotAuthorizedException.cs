using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Authentication.Abstractions.Exceptions;

public class UserNotAuthorizedException : Exception
{
    public UserNotAuthorizedException(string message) : base(message) { }

}
