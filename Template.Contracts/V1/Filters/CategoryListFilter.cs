using Template.Contracts.Attribute;
using Template.Contracts.V1.Models;
using Template.Contracts.V1.Resources;

namespace Template.Contracts.V1.Filters;

public class CategoryListFilter : ListFilter
{
    /// <summary>
    ///     Category Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Order By: Name
    /// </summary>
    [ValidateOrderBy(typeof(CategoryResource))]
    public override string? OrderBy { get; set; }
}