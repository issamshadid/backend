using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Template.Configurations.ExceptionHandler;

/// <summary>
///     Throw BadRequest from the http layer. 400
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class InvalidModelException : ValidateModelException
{
    public InvalidModelException()
    {
    }

    public InvalidModelException(string message) : base(message)
    {
    }

    public InvalidModelException(Dictionary<string, HashSet<string>> errors) : base(errors)
    {
    }

    public InvalidModelException(string key, string error) : base(new Dictionary<string, HashSet<string>>())
    {
        Add(key, error);
    }

    public InvalidModelException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public override int GetStatusCode()
    {
        return StatusCodes.Status400BadRequest;
    }
}