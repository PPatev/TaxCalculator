using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using TaxCalculator.Infrastructure.Resources;

namespace TaxCalculator.Infrastructure.ValidationAttributes
{
    public class IsValidFullName : ValidationAttribute
    {
        private const string _pattern = @"^(?![\s.]+$)[a-zA-Z\s.]*$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string fullName = value as string;
            List<string> names = fullName.Trim().Split(" ", System.StringSplitOptions.RemoveEmptyEntries).ToList();
            bool isValidFullName = names.All(x => Regex.IsMatch(x, _pattern));

            if (names.Count < 2 || !isValidFullName)
            {
                return new ValidationResult(CommonValidations.InvalidFullName);
            }

            return ValidationResult.Success;
        }
    }
}
