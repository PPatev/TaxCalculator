using TaxCalculator.Infrastructure.Configuration;
using TaxCalculator.Infrastructure.Interfaces.Configuration;
using TaxCalculator.Infrastructure.Interfaces.Services;

namespace TaxCalculator.Services
{
    public class IncomeTaxCalculator : IIncomeTaxCalculator
    {
        private readonly TaxCalculatorConfiguration _taxConfiguration;

        public IncomeTaxCalculator(ITaxConfigurationProvider configuration)
        {
            _taxConfiguration = configuration.GetConfiguration();
        }

        /// <summary>
        /// Calculates the total value of the income tax given the gross income and the charity spent by the table person.
        /// </summary>
        /// <param name="grossIncome">The gross income of the taxable person</param>
        /// <param name="charitySpent">The charity spending of the taxable person</param>
        /// <returns>A decimal value representing the toal value of income taxes</returns>
        public decimal CalculateIncomeTax(decimal grossIncome, decimal? charitySpent)
        {
            if (grossIncome <= _taxConfiguration.NonTaxableIncome)
            {
                return default(decimal);
            }

            decimal incomeTaxBase = grossIncome - _taxConfiguration.NonTaxableIncome; 

            decimal charityMaxDeduction = grossIncome * (_taxConfiguration.CharityDeductionMaxPercent / 100M);
            if (charitySpent <= charityMaxDeduction)
            {
                charityMaxDeduction = charitySpent.GetValueOrDefault();
            }

            decimal incomeTaxTotal = (incomeTaxBase - charityMaxDeduction) * (_taxConfiguration.IncomeTaxPercent / 100M);

            return incomeTaxTotal;
        }
    }
}
