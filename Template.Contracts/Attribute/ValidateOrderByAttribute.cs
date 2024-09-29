using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Template.Contracts.Attribute;

/// <summary>
///     this attribute to specify which columns can be used to order by, no custom implementation for now (we just need the
///     type)
/// </summary>
public class ValidateOrderByAttribute : ValidationAttribute
{
    private readonly string[] _alwaysAccept;
    private readonly Type _type;

    public ValidateOrderByAttribute(Type type, params string[] alwaysAccept)
    {
        _type = type;
        _alwaysAccept = alwaysAccept;
    }

    public ValidateOrderByAttribute(string type, params string[] alwaysAccept)
    {
        _alwaysAccept = alwaysAccept;

        var appAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith(type.Split('.').First()))
            .ToArray();

        foreach (var appAssembly in appAssemblies)
        {
            _type = appAssembly.GetType(type)!;
            if (_type != null) break;
        }

        if (_type == null)
            throw new InvalidOperationException($"Type not found for validation. {type}");
    }


    public override bool IsValid(object? value)
    {
        if (value == null) return true;

        //split the orderby string to get the column names to orderby
        var columnsList = value!.ToString()!
            .Split(new[]
            {
                "+",
                "-",
                ",+",
                ",-",
                ","
            }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var invalidColumns = new StringBuilder();

        CheckInvalidColumnsForSql(columnsList, invalidColumns);

        //set the error message to show the invalid columns and valid columns
        if (invalidColumns.Length > 0)
        {
            List<string> orderByColumns;

            orderByColumns = _type.GetProperties()
                .Where(p => IsDefined(p, typeof(IncludeInOrderByAttribute)))
                .Select(p => p.Name)
                .ToList();

            orderByColumns.AddRange(_alwaysAccept);

            if (orderByColumns.Any())
                SetErrorMessageInvalidColumn(
                    $"{invalidColumns}: is/are invalid column(s) to order by. Please use one of these properties: {string.Join(", ", orderByColumns)}");
            else
                SetErrorMessageNoColumn("There is no specific column for order by .");

            return false;
        }

        return true;
    }

    private void CheckInvalidColumnsForSql(List<string> columnsList, StringBuilder invalidColumns)
    {
        foreach (var columnName in columnsList)
        {
            if (_alwaysAccept.Contains(columnName))
                continue;

            if (!_type.GetProperties()
                    .Any(p => string.Equals(p.Name, columnName, StringComparison.CurrentCultureIgnoreCase)
                              && IsDefined(p, typeof(IncludeInOrderByAttribute))))
            {
                if (invalidColumns.Length > 0) invalidColumns.Append(", ");
                invalidColumns.Append(columnName.Trim());
            }
        }
    }

    protected static string FirstCharacterToLower(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str, 0)) return str;
        return char.ToLowerInvariant(str[0]) + str[1..];
    }

    protected string GetProperOrderByFieldName(string fieldName)
    {
        var correctFieldName = _type.GetProperties()
            .FirstOrDefault(p => string.Equals(p.Name, fieldName.ToLower(), StringComparison.CurrentCultureIgnoreCase));
        if (correctFieldName != null) return FirstCharacterToLower(correctFieldName.Name);
        return fieldName;
    }

    protected virtual void SetErrorMessageNoColumn(string text)
    {
        ErrorMessage = text;
    }

    protected virtual void SetErrorMessageInvalidColumn(string text)
    {
        ErrorMessage = text;
    }
}