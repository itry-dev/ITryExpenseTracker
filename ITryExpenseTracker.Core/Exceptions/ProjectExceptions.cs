using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Exceptions;

public class InvalidRoleException : Exception
{
    public InvalidRoleException(string message) : base(message) { }
}
