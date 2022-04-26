using System;
using TaxCalculator.Infrastructure.Interfaces.Models.Entities;

namespace TaxCalculator.Models.Entities
{
    public class IncomeTaxPayer : IIncomeTaxPayer
    {
        public long SSN { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal CharitySpent { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal SocialTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal NetIncome { get; set; }
        public DateTime DateSubsmitted { get; set; } = DateTime.UtcNow;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() != typeof(IncomeTaxPayer)) return false;  

            IncomeTaxPayer other = (IncomeTaxPayer) obj;

            bool areEqual =  SSN.Equals(other.SSN) 
                && FullName == other.FullName 
                && DateOfBirth.Equals(other.DateOfBirth) 
                && GrossIncome.Equals(other.GrossIncome) 
                && IncomeTax.Equals(other.IncomeTax)
                && SocialTax.Equals(other.SocialTax)
                && TotalTax.Equals(other.TotalTax)
                && NetIncome.Equals(other.NetIncome);

            return areEqual;
        }
    }
}
