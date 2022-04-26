﻿using System;
using System.ComponentModel.DataAnnotations;
using TaxCalculator.Infrastructure.Interfaces.Models.InputModels;
using TaxCalculator.Infrastructure.ValidationAttributes;

namespace TaxCalculator.Models.Dtos
{
    public class TaxPayerInputModel : ITaxPayerInputModel
    {
        [Required]
        [IsValidFullName()]
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [NonNegativeDecimal()]
        public decimal GrossIncome { get; set; }

        [Required]
        [Range(10000, 9999999999)]
        public long SSN { get; set; }

        [NonNegativeDecimal()]
        public decimal? CharitySpent { get; set; }
    }
}
