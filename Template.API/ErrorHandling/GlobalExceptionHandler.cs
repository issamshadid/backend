using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Template.Configurations.ExceptionHandler;
using Template.Contracts.V1.Models;

namespace Template.API.ErrorHandling;

/// <summary>
///     Global exception handler.
/// </summary>
/// <remarks>A filter used by the asp.net pipeline.</remarks>
public class GlobalExceptionHandler : IExceptionFilter
{
    private readonly IHostEnvironment _env;

    /// <summary>
    ///     Global Exception Handler
    /// </summary>
    /// <param name="env"></param>
    public GlobalExceptionHandler(IHostEnvironment env)
    {
        _env = env;
    }

    /// <summary>
    ///     On Exception
    /// </summary>
    /// <param name="context"></param>
    public void OnException(ExceptionContext context)
    {
        var result = new ErrorResponse
        {
            TraceId = context.HttpContext.TraceIdentifier
        };
        var statusCode = StatusCodes.Status500InternalServerError;

        switch (context.Exception)
        {
            case IException exception:
                statusCode = exception.GetStatusCode();
                if (exception.GetErrors() is object errors) result.ErrorDetails = errors;
                break;
            case DbUpdateConcurrencyException _:
                statusCode = StatusCodes.Status409Conflict;
                result.ErrorDetails = new Dictionary<string, List<string>>
                {
                    {
                        "timestamp",
                        new List<string> { "Timestamp value is not valid." }
                    }
                };
                break;
        }

        if (!_env.IsProduction())
        {
            result.Message = context.Exception.Message;
            result.StackTrace = context.Exception.ToString();
        }
        else
        {
            result.Message =
                "An error has occurred and has been logged. Use the TraceIdentifier value for any inquires to Template.";
            result.StackTrace = string.Empty;
        }

        context.Result = new ObjectResult(result)
        {
            StatusCode = statusCode,
            DeclaredType = typeof(ErrorResponse)
        };
    }
}