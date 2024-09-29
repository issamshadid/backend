using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Template.Configurations.ExceptionHandler;

/// <summary>
///     Throw forbidden exception from http layer
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class InvalidOperationException : ValidateModelException
{
    public InvalidOperationException()
    {
    }

    public InvalidOperationException(string message) : base(message)
    {
    }

    public InvalidOperationException(Dictionary<string, HashSet<string>> errors) : base(errors)
    {
    }

    public InvalidOperationException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
    public InvalidOperationException(string key, string error) : base(new Dictionary<string, HashSet<string>>())
    {
        Add(key, error);
    }

    public override int GetStatusCode()
    {
        return StatusCodes.Status403Forbidden;
    }
}