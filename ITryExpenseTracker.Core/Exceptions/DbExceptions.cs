using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.Exceptions;

public class ExpenseNotFoundException : Exception
{
    public ExpenseNotFoundException(string message) : base(message) { }
}

public class ExpenseSumException : Exception
{
    public ExpenseSumException(string message) : base(message) { }
}

public class InvalidCategoryIdException : Exception
{
    public InvalidCategoryIdException(string message) : base(message) { }
}

public class RecurringExpenseNotFoundException : Exception
{
    public RecurringExpenseNotFoundException(string message) : base(message) { }
}

public class CategoryExistsException : Exception
{
    public CategoryExistsException(string message) : base(message) { }
}

public class CategoryException : Exception
{
    public CategoryException(string message) : base(message) { }

    public CategoryException(Exception e, string message) : base(message, e) { }
}
