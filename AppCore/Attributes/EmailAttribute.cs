using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AppCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class EmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        try
        {
            var stringValue = (string)value;
            var validateEmailRegex = new Regex(
                "^\\S+@\\S+\\.\\S+$",
                RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled,
                TimeSpan.FromMilliseconds(20)
            );
            var isMatch = validateEmailRegex.IsMatch(stringValue);
            if (stringValue == null ||
                string.IsNullOrEmpty(stringValue) ||
                stringValue.Length == 0 ||
                !isMatch
               )
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
        catch (RegexMatchTimeoutException)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
        catch
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}