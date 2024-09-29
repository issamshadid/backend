namespace Template.Contracts;

public static class ValidationMessages
{
    public static string GetNotFoundMessage(string domainName, params string[] id)
    {
        var isAre = "is";
        var withS = "";
        if (id != null)
        {
            isAre = id.Length > 1
                ? "are"
                : "is";
            withS = id.Length > 1
                ? "s"
                : "";
        }
        else
        {
            id = new[] { "" };
        }

        return $"{domainName} with Id{withS}: " + string.Join(", ", id) + $" {isAre} not found.";
    }

    public static string GetAlreadyExistsMessage(string fieldName, string value)
    {
        return $"{fieldName} {value} already exists.";
    }

    public static string GetNotEmptyMessage(string fieldName)
    {
        return $"{fieldName} can not be empty.";
    }

    public static string GetAlreadyDeletedMessage(string domainName, params string[] id)
    {
        var isAre = "is";
        var withS = "";
        if (id != null)
        {
            isAre = id.Length > 1
                ? "are"
                : "is";
            withS = id.Length > 1
                ? "s"
                : "";
        }
        else
        {
            id = new[] { "" };
        }

        return $"{domainName} with Id{withS}: " + string.Join(", ", id) + $" {isAre} already deleted.";
    }

    public static string GetIsDeletedMessage(string domainName, params string[] id)
    {
        var isAre = "is";
        var withS = "";
        if (id != null)
        {
            isAre = id.Length > 1
                ? "are"
                : "is";
            withS = id.Length > 1
                ? "s"
                : "";
        }
        else
        {
            id = new[] { "" };
        }

        return $"The {domainName} with Id{withS}: " + string.Join(", ", id) + $" {isAre} deleted.";
    }

    public static string GetNotExistsMessage(string domainName, params string[] id)
    {
        var doDose = "is";
        var withS = "";
        if (id != null)
        {
            doDose = id.Length > 1
                ? "do"
                : "dose";
            withS = id.Length > 1
                ? "s"
                : "";
        }
        else
        {
            id = new[] { "" };
        }

        return $"The {domainName} with Id{withS}: " + string.Join(", ", id) + $" {doDose} not exists.";
    }

    public static string GetAlreadyApprovedOrRevokedMessage(string domainName, string code, string name,
        string actionType)
    {
        return $"{domainName} {code} ({name}) is already {actionType} and cannot be updated.";
    }

    public static string GetConcurrencyMessage(string domainName, string code, string name, string modifiedBy,
        string modifiedOn)
    {
        return $"{domainName} {code} ({name}) has been updated by {modifiedBy}, {modifiedOn}";
    }
}