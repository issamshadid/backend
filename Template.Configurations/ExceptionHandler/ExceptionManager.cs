namespace Template.Configurations.ExceptionHandler;

public static class ExceptionManager
{
    public static void ThrowItemNotFoundException(string key, string error)
    {
        throw new ItemNotFoundException(key, error);
    }

    public static void ThrowItemNotFoundException(string error)
    {
        throw new ItemNotFoundException(error);
    }

    public static void ThrowItemNotFoundException(Dictionary<string, HashSet<string>> errors)
    {
        throw new ItemNotFoundException(errors);
    }

    public static void ThrowInvalidModelException(Dictionary<string, HashSet<string>> errors)
    {
        throw new InvalidModelException(errors);
    }

    public static void ThrowInvalidModelException(string key, string error)
    {
        throw new InvalidModelException(key, error);
    }

    public static void ThrowConflictException(string key, string error)
    {
        throw new ConflictException(key, error);
    }

    public static void ThrowConflictException(Dictionary<string, HashSet<string>> errors)
    {
        throw new ConflictException(errors);
    }

    public static void ThrowInvalidOperationException(Dictionary<string, HashSet<string>> errors)
    {
        throw new InvalidOperationException(errors);
    }

    public static void ThrowInvalidOperationException(string key, string error)
    {
        throw new InvalidOperationException(key, error);
    }
}