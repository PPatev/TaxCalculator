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
    public class InputValidationNonNegativeTests
    {
        private TaxPayerInputModel _model;

        [SetUp]
        public void Setup()
        {
            _model = new TaxPayerInputModel();
            _model.FullName = "Peter Patev";
            _model.DateOfBirth = DateTime.Now;
            _model.GrossIncome = 0;
            _model.SSN = "123456789";
            _model.CharitySpent = 0;
        }

        [Test]
        public void NonNegativeDecimalAttribute_WhenCalledWithNegativeGrossIncome_ShouldReturnValidationResultWithCorrectMessage()
        {
            _model.GrossIncome = -1;
            string validationMessage = string.Format(CommonValidations.ResourceManager.GetString("NegativeField"), "GrossIncome");

            List<ValidationResult> result = ValidateModel(_model);

            Assert.IsTrue(result.Any(x => x.ErrorMessage.Equals(validationMessage)));
        }

        [Test]
        public void NonNegativeDecimalAttribute_WhenCalledWithPostiveGrossIncome_ShouldNotReturnValidationResult()
        {
            _model.GrossIncome = 100;
            string validationMessage = string.Format(CommonValidations.ResourceManager.GetString("NegativeField"), "GrossIncome");

            List<ValidationResult> result = ValidateModel(_model);

            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void NonNegativeDecimalAttribute_WhenCalledWithNegativeCharitySpent_ShouldReturnValidationResultWithCorrectMessage()
        {
            _model.CharitySpent = -1;
            string validationMessage = string.Format(CommonValidations.ResourceManager.GetString("NegativeField"), "CharitySpent");

            List<ValidationResult> result = ValidateModel(_model);

            Assert.IsTrue(result.Any(x => x.ErrorMessage.Equals(validationMessage)));
        }

        [Test]
        public void NonNegativeDecimalAttribute_WhenCalledWithPostiveCharitySpent_ShouldNotReturnValidationResult()
        {
            _model.GrossIncome = 100;
            string validationMessage = string.Format(CommonValidations.ResourceManager.GetString("NegativeField"), "GrossIncome");

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
