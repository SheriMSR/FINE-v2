using System.ComponentModel.DataAnnotations;

namespace AppCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MinValueAttribute : ValidationAttribute
{
    public MinValueAttribute(double minValue)
    {
        MinValue = minValue;
    }

    private double MinValue { get; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IComparable comparable && comparable.CompareTo(Convert.ChangeType(MinValue, value.GetType())) > 0)
            return ValidationResult.Success;

        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}