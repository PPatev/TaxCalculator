namespace TaxCalculator.Infrastructure.Configuration
{
    public class TaxCalculatorConfiguration
    {
        public decimal IncomeTaxPercent { get; set; }
        public decimal SocialContributionPercent { get; set; }
        public decimal NonTaxableIncome { get; set; }
        public decimal SocialContributionMaxIncome { get; set; }
        public decimal CharityDeductionMaxPercent { get; set; }
    }
}
