using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaxCalculator.Infrastructure.Resources;
using TaxCalculator.Models.Dtos;



namespace TaxCalculator.UnitTests.ValidationsTests
{
    public class InputValidationFullNameTests
    {
        private TaxPayerInputModel _model;

        [SetUp]
        public void Setup()
        {
            _model = new TaxPayerInputModel();
            _model.DateOfBirth = DateTime.Now;
            _model.GrossIncome = 0;
            _model.SSN = 123456789;
            _model.CharitySpent = 0;
        }

        [Test]
        public void IsValidFullNameAttribute_WhenCalledWithOneName_ShouldReturnValidationResultWithCorrectMessage()
        {
            _model.FullName = "Peter";

            List<ValidationResult> result = ValidateModel(_model);

            Assert.IsTrue(result.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidFullName)));
        }

        [Test]
        public void IsValidFullNameAttribute_WhenCalledWithDigits_ShouldReturnValidationResultWithCorrectMessage()
        {
            _model.FullName = "Peter Patev2";

            List<ValidationResult> result = ValidateModel(_model);

            Assert.IsTrue(result.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidFullName)));
        }

        [Test]
        public void IsValidFullNameAttribute_WhenCalledWithSymbols_ShouldReturnValidationResultWithCorrectMessage()
        {
            _model.FullName = "Pet@r Patev";

            List<ValidationResult> result = ValidateModel(_model);

            Assert.IsTrue(result.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidFullName)));
        }

        [Test]
        public void IsValidFullNameAttribute_WhenCalledWithNonEnglishLetters_ShouldReturnValidationResultWithCorrectMessage()
        {
            _model.FullName = @"Петър Патев";

            List<ValidationResult> result = ValidateModel(_model);

            Assert.IsTrue(result.Any(x => x.ErrorMessage.Equals(CommonValidations.InvalidFullName)));
        }

        [Test]
        public void IsValidFullNameAttribute_WhenCalledWithCorrectName_ShouldNotReturnValidationResult()
        {
            _model.FullName = "Peter Patev";

            List<ValidationResult> result = ValidateModel(_model);

            Assert.That(result.Count, Is.EqualTo(0));
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
