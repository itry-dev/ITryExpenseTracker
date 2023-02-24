using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.OutputModels;
using ITryExpenseTracker.Core.Features.Expenses;
using ITryExpenseTracker.Core.Features.Exceptions;

namespace ITryExpenseTracker.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception is BaseException ? (int)((BaseException)exception).StatusCode : 500;

        var err = new ErrorDetailsOutputModel()
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            ExceptionType = exception.GetType().ToString(),
        };

        if (exception.InnerException != null)
        {
            err.InnerExceptionType = exception.InnerException.GetType().ToString();
            err.InnerMessage = exception.InnerException.Message;
        }

        if (exception.InnerException?.InnerException != null)
        {
            err.InnerInnerExceptionType = exception.InnerException.InnerException.GetType().ToString();
            err.InnerInnerMessage = exception.InnerException.InnerException.Message;
        }

        var jModel = System.Text.Json.JsonSerializer.Serialize<ErrorDetailsOutputModel>(err);

        await context.Response.WriteAsync(jModel);
    }
}
