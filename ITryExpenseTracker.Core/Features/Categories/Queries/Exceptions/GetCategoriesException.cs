using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Features.Categories.Queries.Exceptions;

public class GetCategoriesException : Exception
{
    public GetCategoriesException(string message) : base(message) { }

    public GetCategoriesException(string message, Exception e) : base(message, e) { }
}
