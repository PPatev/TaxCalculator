using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TaxCalculator.Services;

namespace TaxCalculator.UnitTests.CalculatorsTests
{
    public class SocialContributionTests
    {
        private SocialContributionCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            var configuration = new FakeTaxConfigurationProvider();
            _calculator = new SocialContributionCalculator(configuration);
        }

        [Test]
        [TestCase(-1, 0, 0)]
        [TestCase(0, 0, 0)]
        [TestCase(1000, 0, 0)]
        [TestCase(1500, 0, 75)]
        [TestCase(3400, 0, 300)]
        public void CalculateSocialContribution_WhenCalledWithNoCharitySpent_ReturnRightSocialContributionTax(decimal grossIncome, decimal charitySpent, decimal expectedResult)
        {
            decimal result = _calculator.CalculateSocialContribution(grossIncome, charitySpent);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(-1, 150, 0)]
        [TestCase(0, 150, 0)]
        [TestCase(1000, 150, 0)]
        [TestCase(1500, 150, 52.5)]
        [TestCase(3400, 150, 300)]
        [TestCase(3600, 520, 300)]
        public void CalculateSocialContribution_WhenCalledWithCharitySpent_ReturnRightSocialContributionTax(decimal grossIncome, decimal charitySpent, decimal expectedResult)
        {
            decimal result = _calculator.CalculateSocialContribution(grossIncome, charitySpent);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
