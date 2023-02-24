using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.User.Abstractions.Exceptions;
public class UserNotFoundException : Exception {
    public UserNotFoundException(string message) : base(message) { }

    public UserNotFoundException(string message, Exception e) : base(message, e) { }
}

