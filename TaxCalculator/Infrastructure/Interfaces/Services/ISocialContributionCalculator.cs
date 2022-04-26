namespace TaxCalculator.Infrastructure.Interfaces.Services
{
    public interface ISocialContributionCalculator
    {
        /// <summary>
        /// Calculates the total value of the social contribution liability given the gross income and the charity spent of the taxable person
        /// </summary>
        /// <param name="grossIncome">The gross income of the taxable person</param>
        /// <param name="charitySpent">The amount of charity spending of the taxable person</param>
        /// <returns>A decimal value representing the total amount of social contribution liability</returns>
        decimal CalculateSocialContribution(decimal grossIncome, decimal? charitySpent);
    }
}
