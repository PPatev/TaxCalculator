using System;

namespace TaxCalculator.Infrastructure.Interfaces.Models.InputModels
{
    public interface ITaxPayerInputModel
    {
        /// <summary>
        /// The amount of the charity spending (if any) by the taxable person. 
        /// </summary>
        decimal? CharitySpent { get; set; }
        DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Full name of the taxable person. Should be at least two words separated by space, containing only letters.
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        /// The gross income of the taxable person.
        /// </summary>
        decimal GrossIncome { get; set; }
        /// <summary>
        /// Social security number (Unique personal identifier).
        /// </summary>
        long SSN { get; set; }
    }
}