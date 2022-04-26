using System;

namespace TaxCalculator.Infrastructure.Interfaces.Models.Entities
{
    public interface IIncomeTaxEnity
    {
        decimal IncomeTax { get; set; }
        decimal SocialTax { get; set; }
        decimal CharitySpent { get; set; }
        decimal GrossIncome { get; set; }
        decimal NetIncome { get; set; }
        decimal TotalTax { get; set; }
        DateTime DateSubsmitted { get; set; }
    }
}
