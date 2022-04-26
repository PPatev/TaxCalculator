using TaxCalculator.Infrastructure.Interfaces.Models.Entities;
using TaxCalculator.Infrastructure.Interfaces.Models.InputModels;
using TaxCalculator.Infrastructure.Interfaces.Services;
using TaxCalculator.Models.Dtos;
using TaxCalculator.Models.Entities;

namespace TaxCalculator.Services
{
    public class PersonalTaxCalculatorService : IIncomeTaxService
    {
        private readonly IIncomeTaxCalculator _incomeTaxCalculator;
        private readonly ISocialContributionCalculator _socialContributionCalculator;

        public PersonalTaxCalculatorService(
            IIncomeTaxCalculator incomeTaxCalculator,
            ISocialContributionCalculator socialContributionCalculator)
        {
            _incomeTaxCalculator = incomeTaxCalculator;
            _socialContributionCalculator = socialContributionCalculator;
        }
        /// <summary>
        /// Calculates all personal liabilities, including income taxes and social contributions  and also the net income of the taxable person
        /// </summary>
        /// <param name="taxPayerCommand">Accepts an object of type <see cref="ITaxPayerInputModel"/></param>
        /// <returns>An object of type <see cref="IIncomeTaxPayer"/></returns>
        public IIncomeTaxPayer CalculatePersonalTaxes(ITaxPayerInputModel taxPayerCommand)
        {
            decimal totalTax = default(decimal);
            decimal netIncome = taxPayerCommand.GrossIncome;

            IIncomeTaxPayer taxPayerData = new IncomeTaxPayer
            {
                SSN = taxPayerCommand.SSN,
                FullName = taxPayerCommand.FullName,
                DateOfBirth = taxPayerCommand.DateOfBirth,
                GrossIncome = taxPayerCommand.GrossIncome,
                CharitySpent = taxPayerCommand.CharitySpent.GetValueOrDefault(),
                TotalTax = totalTax,
                NetIncome = netIncome,
            };

            decimal incomeTaxTotal = _incomeTaxCalculator.CalculateIncomeTax(taxPayerCommand.GrossIncome, taxPayerCommand.CharitySpent);
            decimal socialContributionTotal = _socialContributionCalculator.CalculateSocialContribution(taxPayerCommand.GrossIncome, taxPayerCommand.CharitySpent);
            totalTax = incomeTaxTotal + socialContributionTotal;
            netIncome -= totalTax;

            taxPayerData.IncomeTax = incomeTaxTotal;
            taxPayerData.SocialTax = socialContributionTotal;
            taxPayerData.TotalTax = totalTax;
            taxPayerData.NetIncome = netIncome;

            return taxPayerData;
        }
    }
}
