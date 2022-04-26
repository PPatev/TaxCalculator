using TaxCalculator.Infrastructure.Configuration;
using TaxCalculator.Infrastructure.Interfaces.Configuration;
using TaxCalculator.Infrastructure.Interfaces.Services;

namespace TaxCalculator.Services
{
    public class SocialContributionCalculator : ISocialContributionCalculator
    {

        private readonly TaxCalculatorConfiguration _taxConfiguration;

        public SocialContributionCalculator(ITaxConfigurationProvider configuration)
        {
            _taxConfiguration = configuration.GetConfiguration();
        }

        /// <summary>
        /// Calculates the total value of the social contribution liability given the gross income and the charity spent of the taxable person
        /// </summary>
        /// <param name="grossIncome">The gross income of the taxable person</param>
        /// <param name="charitySpent">The amount of charity spending of the taxable person</param>
        /// <returns>A decimal value representing the total amount of social contribution liability</returns>
        public decimal CalculateSocialContribution(decimal grossIncome, decimal? charitySpent)
        {
            decimal socialContributionBase = default(decimal);

            if (grossIncome <= _taxConfiguration.NonTaxableIncome)
            {
                return socialContributionBase;
            }

            decimal charityDeduction = grossIncome * (_taxConfiguration.CharityDeductionMaxPercent / 100M);

            if (charitySpent <= charityDeduction)
            {
                charityDeduction = charitySpent.GetValueOrDefault();
            }

            grossIncome -= charityDeduction;

            if (grossIncome > _taxConfiguration.SocialContributionMaxIncome)
            {
                socialContributionBase = _taxConfiguration.SocialContributionMaxIncome - _taxConfiguration.NonTaxableIncome;
            }
            else
            {
                socialContributionBase = grossIncome - _taxConfiguration.NonTaxableIncome;
            }
            
            decimal socialContributionTotal = socialContributionBase * (_taxConfiguration.SocialContributionPercent / 100M);

            return socialContributionTotal;
        }
    }
}
