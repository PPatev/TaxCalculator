using TaxCalculator.Infrastructure.Interfaces.Models.Entities;
using TaxCalculator.Infrastructure.Interfaces.Models.InputModels;

namespace TaxCalculator.Infrastructure.Interfaces.Services
{
    public interface IIncomeTaxService
    {
        /// <summary>
        /// Calculates all personal liabilities, including income taxes and social contributions  and also the net income of the taxable person
        /// </summary>
        /// <param name="taxPayerCommand">Accepts an object of type <see cref="ITaxPayerInputModel"/></param>
        /// <returns>An object of type <see cref="IIncomeTaxPayer"/></returns>
        IIncomeTaxPayer CalculatePersonalTaxes(ITaxPayerInputModel model);
    }
}
