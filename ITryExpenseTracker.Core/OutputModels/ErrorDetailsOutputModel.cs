using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Core.OutputModels;

public class ErrorDetailsOutputModel
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = "";

    public string ExceptionType { get; set; } = "";

    public string InnerMessage { get; set; } = "";

    public string InnerExceptionType { get; set; } = "";

    public string InnerInnerMessage { get; set; } = "";

    public string InnerInnerExceptionType { get; set; } = "";
}
