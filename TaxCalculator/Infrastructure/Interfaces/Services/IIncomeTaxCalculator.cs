namespace TaxCalculator.Infrastructure.Interfaces.Services
{
    public interface IIncomeTaxCalculator
    {
        /// <summary>
        /// Calculates the total value of the income tax given the gross income and the charity spent by the table person.
        /// </summary>
        /// <param name="grossIncome">The gross income of the taxable person</param>
        /// <param name="charitySpent">The charity spending of the taxable person</param>
        /// <returns>A decimal value representing the toal value of income taxes</returns>
        decimal CalculateIncomeTax(decimal grossIncome, decimal? charitySpent);
    }
}
