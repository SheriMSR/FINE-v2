using System.ComponentModel.DataAnnotations;

namespace AppCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MaxLengthAttribute : ValidationAttribute
{
    public MaxLengthAttribute(int length)
    {
        Length = length;
    }

    private int Length { get; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value.ToString()?.Length > Length)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        return ValidationResult.Success;
    }
}