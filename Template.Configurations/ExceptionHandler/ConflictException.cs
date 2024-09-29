using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Template.Configurations.ExceptionHandler;

[Serializable]
[ExcludeFromCodeCoverage]
public class ConflictException : ValidateModelException
{
    public ConflictException()
    {
    }

    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ConflictException(string key, string error) : base(new Dictionary<string, HashSet<string>>())
    {
        Add(key, error);
    }

    public ConflictException(Dictionary<string, HashSet<string>> errors) : base(errors)
    {
        Errors = errors;
    }

    public override int GetStatusCode()
    {
        return StatusCodes.Status409Conflict;
    }
}