using System.ComponentModel.DataAnnotations;

namespace AppCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MaxCountListAttribute : ValidationAttribute
{
    public MaxCountListAttribute(int length)
    {
        Length = length;
    }

    private int Length { get; }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is List<object> list && list.Count > Length)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        return ValidationResult.Success!;
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MinCountListAttribute : ValidationAttribute
{
    public MinCountListAttribute(int length)
    {
        Length = length;
    }

    private int Length { get; }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is List<object> list && list.Count < Length)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        return ValidationResult.Success!;
    }
}