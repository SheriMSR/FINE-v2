using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AppCore.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MinYearAttribute : ValidationAttribute
{
    public MinYearAttribute(int minYear)
    {
        MinYear = minYear;
    }

    private int MinYear { get; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        try
        {
            var now = DateTime.Now;
            var datetime = (DateTime)value;
            if (datetime.AddYears(MinYear).Date >= now.Date)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
        catch (RegexMatchTimeoutException)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
        catch (Exception)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}