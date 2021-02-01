using Calculator.Infrastructure.Interfaces;
using Calculator.Infrastructure.Services;
using NUnit.Framework;

namespace Calculator.Tests
{
    [TestFixture]
    public class CalculationServiceTests
    {
        private ICalculationService _calculationService;
        [SetUp]
        public void SetUp()
        {
            _calculationService = new CalculationService();
        }

        [Test]
        public void Calculate_ValidInputPlusAndMultiple_RightResult()
        {
            var input = "5+6*7";
            var expectedResult = "47";

            var result = _calculationService.Calculate(input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Calculate_ValidInputPlus_RightResult()
        {
            var input = "5+6";
            var expectedResult = "11";

            var result = _calculationService.Calculate(input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Calculate_ValidInputMinus_RightResult()
        {
            var input = "5-6";
            var expectedResult = "-1";

            var result = _calculationService.Calculate(input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Calculate_ValidInputNegativeNumbersMultiple_RightResult()
        {
            var input = "-5*-6";
            var expectedResult = "30";

            var result = _calculationService.Calculate(input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Calculate_ValidInputNegativeNumbersDivide_RightResult()
        {
            var input = "-30/-6";
            var expectedResult = "5";

            var result = _calculationService.Calculate(input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Calculate_ValidInputMoreThanLong_RightResult()
        {
            var input = "3012312321312521312*1231221312312321+123125321312312312-1231221321312312312+12312321312312*141321332112";

            Assert.That(()=>_calculationService.Calculate(input),Throws.Nothing);
        }

        [Test]
        public void Calculate_ValidInputComplex_RightResult()
        {
            var input = "-30/-6+356*1-125/5";
            var expectedResult = "336";

            var result = _calculationService.Calculate(input);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
