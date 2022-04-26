using System.ComponentModel.DataAnnotations;
using TaxCalculator.Infrastructure.Resources;

namespace TaxCalculator.Infrastructure.ValidationAttributes
{
    public class NonNegativeDecimal : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            decimal? num = value as decimal?;
            if (num.GetValueOrDefault() < 0)
            {
                string fieldName = validationContext.MemberName;
                return new ValidationResult(string.Format(CommonValidations.ResourceManager.GetString("NegativeField"), fieldName));
            }

            return ValidationResult.Success;
        }
    }
}
