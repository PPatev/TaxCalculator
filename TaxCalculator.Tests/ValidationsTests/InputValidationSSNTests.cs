using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TaxCalculator.Models.Dtos;

namespace TaxCalculator.UnitTests.ValidationsTests
{
    public class InputValidationSSNTests
    {
        private TaxPayerInputModel _model;

        [SetUp]
        public void Setup()
        {
            _model = new TaxPayerInputModel();
            _model.FullName = "Peter Patev";
            _model.DateOfBirth = DateTime.Now;
            _model.GrossIncome = 0;
            _model.SSN = 123456789;
            _model.CharitySpent = 0;
        }

        [Test]
        public void RangeAttribute_WhenCalledWithLessThanFiveDigits_ShouldReturnFalse()
        {
            _model.SSN = 1234;
           
            bool result = ValidateModel(_model);

            Assert.IsFalse(result);
        }

        [Test]
        public void RangeAttribute_WhenCalledWithMoreThanNineDigits_ShouldReturnFalse()
        {
            _model.SSN = 12345678910;

            bool result = ValidateModel(_model);

            Assert.IsFalse(result);
        }

        [Test]
        public void RangeAttribute_WhenCalledWithNineDigits_ShouldReturnTrue()
        {
            _model.SSN = 123456789;

            bool result = ValidateModel(_model);

            Assert.IsTrue(result);
        }

        [Test]
        public void RangeAttribute_WhenCalledWithFiveDigits_ShouldReturnTrue()
        {
            _model.SSN = 12345;

            bool result = ValidateModel(_model);

            Assert.IsTrue(result);
        }

        [Test]
        public void RangeAttribute_WhenCalledWithNegative_ShouldReturnFalse()
        {
            _model.SSN = -12345;

            bool result = ValidateModel(_model);

            Assert.IsFalse(result);
        }

        private bool ValidateModel(object model)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(model, null, null);
            bool result = Validator.TryValidateObject(model, validationContext, validationResults, true);
            return result;
        }
    }
}
