using System.ComponentModel.DataAnnotations;

namespace Template.Contracts.Attribute;

[AttributeUsage(AttributeTargets.Property)]
public class HybridFromRouteAttribute : ValidationAttribute
{
    public HybridFromRouteAttribute(string routeParameterName)
    {
        RouteParameterName = routeParameterName;
    }

    public string RouteParameterName { get; }

    public override bool IsValid(object? value)
    {
        return true;
    }
}