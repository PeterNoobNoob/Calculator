using System;
using System.Collections.Generic;
using System.Text;
using Calculator.Infrastructure.Interfaces;
using Calculator.Infrastructure.Services;
using NUnit.Framework;

namespace Calculator.Tests
{
    [TestFixture]
    public class ValidatorServiceTests
    {
        private IValidatorService _validatorService;
        [SetUp]
        public void SetUp()
        {
            _validatorService = new ValidatorService();
        }

        [Test]
        public void ValidateCalculatorInput_ValidInput_ErrorListEmpty()
        {
            var calculatorInput = "6+5";

            var errors = _validatorService.ValidateCalculatorInput(calculatorInput);

            Assert.IsEmpty(errors);
        }

        [Test]
        public void ValidateCalculatorInput_InValidInputDouble_ErrorListNotEmpty()
        {
            var calculatorInput = "6+5.1";

            var errors = _validatorService.ValidateCalculatorInput(calculatorInput);

            Assert.IsNotEmpty(errors);
        }

        [Test]
        public void ValidateCalculatorInput_InValidInputVariable_ErrorListNotEmpty()
        {
            var calculatorInput = "6+a";

            var errors = _validatorService.ValidateCalculatorInput(calculatorInput);

            Assert.IsNotEmpty(errors);
        }
    }
}
