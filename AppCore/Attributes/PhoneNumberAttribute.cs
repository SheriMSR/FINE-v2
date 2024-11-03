using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AppCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class PhoneNumberAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        try
        {
            var stringValue = (string)value;
            var validatePhoneNumberRegex = new Regex(
                "(84|[03|05|07|08|09]|[3|5|7|8|9])+([0-9]{7})",
                RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled,
                TimeSpan.FromMilliseconds(20)
            );
            var isMatch = validatePhoneNumberRegex.IsMatch(stringValue);
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