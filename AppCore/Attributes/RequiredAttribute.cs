using System.ComponentModel.DataAnnotations;

namespace AppCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class RequiredAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null ||
            string.IsNullOrEmpty(value.ToString()) ||
            string.IsNullOrWhiteSpace(value.ToString()) ||
            value.ToString()?.Length == 0
           )
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        return ValidationResult.Success;
    }
}