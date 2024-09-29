using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Template.Configurations.ExceptionHandler;

namespace Template.API.ErrorHandling;

/// <summary>
///     Global exception logger.
/// </summary>
/// <remarks>A filter used by the asp.net pipeline.</remarks>
public class GlobalExceptionLogger : IExceptionFilter
{
    private static readonly int[] StatusCodesTreatedAsWarning =
    {
        StatusCodes.Status404NotFound,
        StatusCodes.Status400BadRequest,
        StatusCodes.Status409Conflict,
        StatusCodes.Status403Forbidden,
        StatusCodes.Status401Unauthorized,
        StatusCodes.Status410Gone
    };

    private readonly ILogger<GlobalExceptionLogger> _logger;

    /// <summary>
    ///     Global Exception Logger
    /// </summary>
    /// <param name="logger"></param>
    public GlobalExceptionLogger(ILogger<GlobalExceptionLogger> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     On Exception
    /// </summary>
    /// <param name="context"></param>
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case IException exception
                when StatusCodesTreatedAsWarning.Contains(exception.GetStatusCode()):
                Warning(context, exception);
                break;
            case IException exception:
                Error(context, exception);
                break;
            default:
                Error(context);
                break;
        }
    }

    private void Warning(ExceptionContext context, IException exception)
    {
        _logger.Warning(
            context.Exception,
            "{ControllerName}Controller > {ActionName}: {@Errors}",
            context.RouteData.Values["controller"]!,
            context.RouteData.Values["action"]!,
            exception.GetErrors()!);
    }

    private void Error(ExceptionContext context, IException exception)
    {
        _logger.Error(
            context.Exception,
            "{ControllerName}Controller > {ActionName}: {@Errors}",
            context.RouteData.Values["controller"]!,
            context.RouteData.Values["action"]!,
            exception.GetErrors()!);
    }

    private void Error(ExceptionContext context)
    {
        _logger.Error(
            context.Exception,
            "{ControllerName}Controller > {ActionName}. Unexpected error: {ErrorMessage}",
            context.RouteData.Values["controller"]!,
            context.RouteData.Values["action"]!,
            GetExceptionMessage(context.Exception));
    }

    private static string GetExceptionMessage(Exception exception)
    {
        var stringBuilder = new StringBuilder($"\"{exception.Message}\"");
        if (exception is ValidateModelException modelException && modelException.Errors.Any())
        {
            stringBuilder.Append(", exception data: \"");
            stringBuilder.Append(string.Join(", ",
                modelException.Errors.Select(x => $"{x.Key}:[{string.Join(',', x.Value)}]")));
            stringBuilder.Append('"');
        }

        return stringBuilder.ToString();
    }
}