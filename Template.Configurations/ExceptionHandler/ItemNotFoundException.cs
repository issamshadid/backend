using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Template.Configurations.ExceptionHandler;

/// <summary>
///     Throw Notfound result from the http layer. 404
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class ItemNotFoundException : ValidateModelException
{
    public ItemNotFoundException()
    {
    }

    public ItemNotFoundException(string message) : base(message)
    {
    }

    public ItemNotFoundException(Dictionary<string, HashSet<string>> errors) : base(errors)
    {
    }

    public ItemNotFoundException(string key, string error) : base(new Dictionary<string, HashSet<string>>())
    {
        Add(key, error);
    }

    public ItemNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public override string Message
    {
        get
        {
            var builder = new StringBuilder();
            builder.Append(base.Message);

            if (Errors.Count > 0) builder.AppendLine();

            foreach (var error in Errors) builder.AppendLine($"{error.Key}: {string.Join(". ", error.Value)}.");

            return builder.ToString();
        }
    }

    public override int GetStatusCode()
    {
        return StatusCodes.Status404NotFound;
    }
}