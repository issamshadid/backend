using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Template.Configurations.ExceptionHandler;

public interface IException
{
    int GetStatusCode();

    object? GetErrors();
}

[Serializable]
public abstract class ValidateModelException : ApplicationException, IException
{
    protected ValidateModelException()
    {
        // Empty constructor required to compile.
    }

    protected ValidateModelException(string message) : base(message)
    {
    }

    protected ValidateModelException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ValidateModelException(Dictionary<string, HashSet<string>> errors)
    {
        Errors = errors;
    }

    public Dictionary<string, HashSet<string>> Errors { get; set; } = new();

    public abstract int GetStatusCode();

    public object GetErrors()
    {
        return Errors;
    }

    // Implement custom serialization using JSON
    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }

    public HashSet<string> GetErrorString(
        string key)
    {
        Errors.TryGetValue(key, out var errorString);
        return errorString!;
    }

    public void Add(
        string key,
        string value)
    {
        if (Errors.ContainsKey(key))
            Errors[key].Add(value);
        else
            Errors.Add(key, new HashSet<string> { value });
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder("\n");
        if (Errors.Count > 0)
        {
            stringBuilder.Append("exception data: \"");
            foreach (var error in Errors)
            {
                stringBuilder.Append(error.Key);
                stringBuilder.Append(":[");
                stringBuilder.Append(string.Join(',', error.Value));
                stringBuilder.Append(']');
            }

            stringBuilder.Append("\"\n");
        }

        return stringBuilder + base.ToString();
    }
}