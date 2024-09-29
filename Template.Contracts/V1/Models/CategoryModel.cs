using System.ComponentModel.DataAnnotations;

namespace Template.Contracts.V1.Models;

public class CategoryModel
{
    /// <summary>
    ///     Category name
    /// </summary>
    [Required]
    [MaxLength(Constants.NameLength)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Other Category name
    /// </summary>
    [Required]
    [MaxLength(Constants.NameLength)]
    public string OtherName { get; set; } = string.Empty;
}