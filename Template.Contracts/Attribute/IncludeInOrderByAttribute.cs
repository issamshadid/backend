using System.Runtime.CompilerServices;

namespace Template.Contracts.Attribute;

/// <summary>
///     this attribute to specify which columns can be used to order by.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class IncludeInOrderByAttribute : System.Attribute
{
    /// <summary>
    ///     use entityPropertyName to specify the porperty name in the entity
    /// </summary>
    /// <param name="entityPropertyName">the name of the property in the entity if different</param>
    /// <param name="propertyName"> the property name</param>
    public IncludeInOrderByAttribute(string? entityPropertyName = null,
        [CallerMemberName] string? propertyName = null)
    {
        EntityPropertyName = entityPropertyName;

        if (entityPropertyName == null)
            EntityPropertyName = propertyName;
    }

    public string? EntityPropertyName { get; set; }
}