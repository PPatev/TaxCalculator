using Moq;
using NUnit.Framework;
using System;
using TaxCalculator.Infrastructure.Interfaces.Models.Entities;
using TaxCalculator.Infrastructure.Interfaces.Services;
using TaxCalculator.Models.Dtos;
using TaxCalculator.Models.Entities;
using TaxCalculator.Services;

namespace TaxCalculator.UnitTests.TaxServiceTests
{
    public class PersonalTaxCalculatorServiceTests
    {
        [Test]
        [TestCase(0, 0, 0, 0)]
        public void CalculatePersonalTaxes_WhenCalled_ShouldReturnType(decimal grossIncome, decimal charitySpent, decimal incomeTaxResult, decimal socialContributionResult)
        {
            Mock<IIncomeTaxCalculator> mockIncomeTaxCalculator = new Mock<IIncomeTaxCalculator>();
            mockIncomeTaxCalculator.Setup(x => x.CalculateIncomeTax(grossIncome, charitySpent)).Returns(incomeTaxResult);

            Mock<ISocialContributionCalculator> mockSocialContributionCalculator = new Mock<ISocialContributionCalculator>();
            mockSocialContributionCalculator.Setup(x => x.CalculateSocialContribution(grossIncome, charitySpent)).Returns(socialContributionResult);

            PersonalTaxCalculatorService taxCalculator = new PersonalTaxCalculatorService(mockIncomeTaxCalculator.Object, mockSocialContributionCalculator.Object);
            TaxPayerInputModel inputModel = SetUpInputModel(grossIncome, charitySpent);

            Assert.IsInstanceOf<IncomeTaxPayer>(taxCalculator.CalculatePersonalTaxes(inputModel));
        }

        [Test]
        [TestCase(0, 0, 0, 0, 0, 0)]
        [TestCase(980, 0, 0, 0, 0, 980)]
        [TestCase(3400, 0, 240, 300, 540, 2860)]
        [TestCase(2500, 150, 135, 202.5, 337.5, 2162.5)]
        [TestCase(3600, 520, 224, 300, 524, 3076)]
        public void CalculatePersonalTaxes_WhenCalled_ShouldReturnModelWithCorrectValues(
            decimal grossIncome, 
            decimal charitySpent, 
            decimal incomeTaxResult, 
            decimal socialContributionResult, 
            decimal totalTaxResult, 
            decimal netIncomeResult)
        {
            Mock<IIncomeTaxCalculator> mockIncomeTaxCalculator = new Mock<IIncomeTaxCalculator>();
            mockIncomeTaxCalculator.Setup(x => x.CalculateIncomeTax(grossIncome, charitySpent)).Returns(incomeTaxResult);

            Mock<ISocialContributionCalculator> mockSocialContributionCalculator = new Mock<ISocialContributionCalculator>();
            mockSocialContributionCalculator.Setup(x => x.CalculateSocialContribution(grossIncome, charitySpent)).Returns(socialContributionResult);

            PersonalTaxCalculatorService taxCalculator = new PersonalTaxCalculatorService(mockIncomeTaxCalculator.Object, mockSocialContributionCalculator.Object);
            TaxPayerInputModel inputModel = SetUpInputModel(grossIncome, charitySpent);
            IIncomeTaxPayer taxPayer = taxCalculator.CalculatePersonalTaxes(inputModel);

            Assert.That(taxPayer.TotalTax, Is.EqualTo(totalTaxResult));
            Assert.That(taxPayer.NetIncome, Is.EqualTo(netIncomeResult));
        }

        private TaxPayerInputModel SetUpInputModel(decimal grossIncome, decimal charitySpent)
        {
            return new TaxPayerInputModel
            {
                FullName = "Peter Patev",
                DateOfBirth = DateTime.UtcNow,
                SSN = 12345678,
                GrossIncome = grossIncome,
                CharitySpent = charitySpent
            };
        }
    }
}
