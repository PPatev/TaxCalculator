using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TaxCalculator.Infrastructure.Resources;
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
            _model.SSN = null;
            _model.CharitySpent = 0;
        }

        [Test]
        public void IsValidSSN_WhenCalledWithNull_ShouldReturnValidationResult()
        {
            List<ValidationResult> results = ValidateModel(_model);

            Assert.IsTrue(results.Any(x => x.ErrorMessage.Equals("The SSN field is required.")));
        }

        [Test]
        public void IsValidSSN_WhenCalledWithEmpty_ShouldReturnValidationResult()
        {
            _model.SSN = "";

            List<ValidationResult> results = ValidateModel(_model);

            Assert.IsTrue(results.Any(x => x.ErrorMessage.Equals("The SSN field is required.")));
        }

        [Test]
        public void IsValidSSN_WhenCalledWithSpaces_ShouldReturnValidationResult()
        {
            _model.SSN = "123 456";

            List<ValidationResult> results = ValidateModel(_model);

            Assert.IsTrue(results.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidSSN)));
        }

        [Test]
        public void IsValidSSN_WhenCalledWithLetters_ShouldReturnValidationResult()
        {
            _model.SSN = "number";

            List<ValidationResult> results = ValidateModel(_model);

            Assert.IsTrue(results.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidSSN)));
        }

        [Test]
        public void IsValidSSN_WhenCalledWithLettersAndDigits_ShouldReturnValidationResult()
        {
            _model.SSN = "numb567";

            List<ValidationResult> results = ValidateModel(_model);

            Assert.IsTrue(results.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidSSN)));
        }

        [Test]
        public void IsValidSSN_WhenCalledWithLessThanFiveDigits_ShouldReturnValidationResult()
        {
            _model.SSN = "1234";

            List<ValidationResult> results = ValidateModel(_model);

            Assert.IsTrue(results.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidSSN)));
        }

        [Test]
        public void IsValidSSN_WhenCalledWithMoreThanNineDigits_ShouldReturnValidationResult()
        {
            _model.SSN = "12345678910";

            List<ValidationResult> results = ValidateModel(_model);

            Assert.IsTrue(results.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidSSN)));
        }

        [Test]
        public void IsValidSSN_WhenCalledWithNineDigits_ShouldReturnValidationResultSuccess()
        {
            _model.SSN = "123456789";

            List<ValidationResult> results = ValidateModel(_model);

            Assert.That(results.Count, Is.EqualTo(0));
        }

        [Test]
        public void IsValidSSN_WhenCalledWithFiveDigits_ShouldReturnValidationResultSuccess()
        {
            _model.SSN = "12345";

            List<ValidationResult> results = ValidateModel(_model);

            Assert.That(results.Count, Is.EqualTo(0));
        }

        [Test]
        public void IsValidSSN_WhenCalledWithNegative_ShouldReturnValidationResult()
        {
            _model.SSN = "-12345";

            List<ValidationResult> results = ValidateModel(_model);

            Assert.IsTrue(results.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidSSN)));
        }

        private List<ValidationResult> ValidateModel(object model)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            return validationResults;
        }
    }
}
