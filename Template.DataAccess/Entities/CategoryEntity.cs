using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Template.Contracts;

namespace Template.DataAccess.Entities;

[Table("Categories")]
public class CategoryEntity : BaseEntity
{
    /// <summary>
    ///     Category Name
    /// </summary>
    [Required]
    [MaxLength(Constants.NameLength)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Other Category Name
    /// </summary>
    [Required]
    [MaxLength(Constants.NameLength)]
    public string OtherName { get; set; } = string.Empty;
}