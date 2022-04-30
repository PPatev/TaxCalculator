using System;

namespace TaxCalculator.Infrastructure.Interfaces.Models.Entities
{
    public interface ITaxEntity
    {
       string SSN { get; set; }
       string FullName { get; set; }
       DateTime? DateOfBirth { get; set; }
    }
}
