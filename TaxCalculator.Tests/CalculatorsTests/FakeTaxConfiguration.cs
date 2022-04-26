using TaxCalculator.Infrastructure.Configuration;
using TaxCalculator.Infrastructure.Interfaces.Configuration;

namespace TaxCalculator.UnitTests.CalculatorsTests
{
    internal class FakeTaxConfigurationProvider : ITaxConfigurationProvider
    {
        public TaxCalculatorConfiguration GetConfiguration()
        {
            return new TaxCalculatorConfiguration
            {
                IncomeTaxPercent = 10M,
                SocialContributionPercent = 15M,
                NonTaxableIncome = 1000M,
                SocialContributionMaxIncome = 3000M,
                CharityDeductionMaxPercent = 10M
            };
        }
    }
}
