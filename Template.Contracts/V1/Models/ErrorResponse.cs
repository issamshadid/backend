namespace Template.Contracts.V1.Models;

/// <summary>
///     Error result that ultimately is seen by the api consumers.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    ///     W3C TraceIdentifier
    /// </summary>
    public string TraceId { get; set; } = string.Empty;

    /// <summary>
    ///     Error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    public object ErrorDetails { get; set; } = Array.Empty<object>();

    /// <summary>
    ///     Exception stack trace.
    /// </summary>
    public string StackTrace { get; set; } = string.Empty;
}

public class ErrorResponse<T> : ErrorResponse
{
    public new T ErrorDetails { get; set; } = default!;
}