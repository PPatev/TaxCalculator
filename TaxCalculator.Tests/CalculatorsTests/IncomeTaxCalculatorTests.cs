using NUnit.Framework;
using TaxCalculator.Services;

namespace TaxCalculator.UnitTests.CalculatorsTests
{
    public class IncomeTaxCalculatorTests
    {
        private IncomeTaxCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            var configuration = new FakeTaxConfigurationProvider();
            _calculator = new IncomeTaxCalculator(configuration);
        }

        [Test]
        [TestCase(-1, 0, 0)]
        [TestCase(0, 0, 0)]
        [TestCase(1000, 0, 0)]
        [TestCase(1500, 0, 50)]
        [TestCase(3400, 0, 240)]
        public void CalculateIncomeTax_WhenCalledWithNoCharitySpent_ReturnRightIncomeTax(decimal grossIncome, decimal charitySpent, decimal expectedResult)
        {
            decimal result = _calculator.CalculateIncomeTax(grossIncome, charitySpent);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(-1, 150, 0)]
        [TestCase(0, 150, 0)]
        [TestCase(1000, 150, 0)]
        [TestCase(1500, 150, 35)]
        [TestCase(3400, 150, 225)]
        [TestCase(3600, 520, 224)]
        public void CalculateIncomeTax_WhenCalledWithCharitySpent_ReturnRightIncomeTax(decimal grossIncome, decimal charitySpent, decimal expectedResult)
        {
            decimal result = _calculator.CalculateIncomeTax(grossIncome, charitySpent);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}