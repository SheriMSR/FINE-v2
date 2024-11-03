using System.ComponentModel.DataAnnotations;

namespace AppCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MaxValueAttribute : ValidationAttribute
{
    public MaxValueAttribute(double maxValue)
    {
        MaxValue = maxValue;
    }

    private double MaxValue { get; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IComparable comparable && comparable.CompareTo(Convert.ChangeType(MaxValue, value.GetType())) < 0)
            return ValidationResult.Success;

        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}