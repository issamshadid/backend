using Template.Contracts.Attribute;

namespace Template.Contracts.V1.Resources;

public class CategoryResource
{
    public int Id { get; set; }

    [IncludeInOrderBy] public string Name { get; set; } = string.Empty;

    [IncludeInOrderBy] public string OtherName { get; set; } = string.Empty;
}