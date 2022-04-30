using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using TaxCalculator.Infrastructure.Resources;

namespace TaxCalculator.Infrastructure.ValidationAttributes
{
    public class IsValidSSN : ValidationAttribute
    {
        private const string _pattern = @"^[0-9]{5,9}$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(CommonValidations.InvalidSSN);
            }

            string ssn = value as string;
            bool isValidSSN = Regex.IsMatch(ssn.Trim(), _pattern);

            if (!isValidSSN)
            {
                return new ValidationResult(CommonValidations.InvalidSSN);
            }

            return ValidationResult.Success;
        }
    }
}
