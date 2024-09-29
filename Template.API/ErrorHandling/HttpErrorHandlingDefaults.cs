using Microsoft.AspNetCore.Mvc;
using Template.Contracts.V1.Models;

namespace Template.API.ErrorHandling;

/// <summary>
///     Http Error Handling Defaults
/// </summary>
public static class HttpErrorHandlingDefaults
{
    /// <summary>
    ///     Create Invalid Model State Response
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IActionResult CreateInvalidModelStateResponse(ActionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ErrorResponse>>();
        var errors = context.ModelState
            .ToDictionary(x => x.Key,
                x => x.Value?.Errors.Select(y => y.ErrorMessage));

        logger.Warning("{controllerName} Controller > {actionName}: Invalid request event {@ErrorDetails}",
            context.RouteData.Values["controller"]!.ToString()!,
            context.RouteData.Values["action"]!.ToString()!,
            errors);

        var result = new ErrorResponse
        {
            TraceId = context.HttpContext.TraceIdentifier,
            Message = "One or more validation errors occurred.",
            ErrorDetails = errors
        };

        return new ObjectResult(result)
        {
            StatusCode = StatusCodes.Status400BadRequest,
            DeclaredType = typeof(ErrorResponse)
        };
    }
}