namespace Template.API.Infrastructure;

/// <summary>
///     Language Middleware
/// </summary>
public class LanguageMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Language Middleware
    /// </summary>
    /// <param name="next"></param>
    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///     Invoke Async
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Accept-Language"))
        {
            var language = context.Request.Headers["Accept-Language"].ToString();
            context.Items["Accept-Language"] = language;
        }

        await _next(context);
    }
}