using System.ComponentModel.DataAnnotations;

namespace TamTam.Contracts.V1.Models;

public interface IListFilter
{
    public int? Skip { get; }
    public int? Take { get; }
    public string? OrderBy { get; }
}

public class ListFilter : IValidatableObject, IListFilter
{
    /// <summary>
    ///     Skip first 'skip' user(s)
    /// </summary>
    [Range(0, int.MaxValue)]
    public int? Skip { get; set; }

    /// <summary>
    ///     Take first 'take' users(s)
    /// </summary>
    [Range(1, 9999)]
    public int? Take { get; set; }

    /// <summary>
    ///     Comma separated properties names. Use '+' with column name for ascending and '-' for descending order
    ///     example: +col1,-col2,+col3,...
    ///     Supported names are: createdOn, createdBy, modifiedOn, modifiedBy
    /// </summary>
    [RegularExpression(@"^[0-9A-Za-z+,\-\. ]*$",
        ErrorMessage = "Only letters, numbers and '+', '-', ',', '.', are allowed for order by property.")]
    public virtual string? OrderBy { get; set; }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var result = new ValidationResult[1];
        // if sum is less than 0 => an overflow occurred
        if ((Skip ?? 0) + (Take ?? 0) < 0)
            result[0] = new ValidationResult($"Sum of skip and take cannot be greater than {int.MaxValue}",
                new[] { "Skip", "Take" });

        return result;
    }

    public T Clone<T>() where T : ListFilter
    {
        return (T)MemberwiseClone();
    }
}