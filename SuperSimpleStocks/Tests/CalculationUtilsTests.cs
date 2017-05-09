using System;
using SuperSimpleStocks.Utils;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace SuperSimpleStocks.Tests
{
    [TestFixture]
    public class CalculationUtilsTests
    {
        #region CalculationUtils Tests
        [Test]
        public void EnsureCalculateProductFromValuesReturnsCorrectValue()
        {
            double[] values = { 1, 2, 3, 4 };
            double expectedResult = 24;

            var result = CalculationUtils.CalculateProductFromValues(values);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void EnsureCalculateNRootReturnsCorrectValue()
        {
            double inputValue = 256;
            int n = 4;
            double expectedValue = 4;

            var result = CalculationUtils.CalculateNRoot(n, inputValue);

            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        [TestCase(new double[] { 8, 18 }, 12)]
        [TestCase(new double[] { 4, 8, 3, 9, 17 }, 6.81)]
        [TestCase(new double[] { 45, 58, 105, 60, 15 }, 47.69)]
        public void EnsureGeometricMeanReturnsCorrectValue(double[] values, double expectedResult)
        {
            var result = Math.Round(CalculationUtils.CalculateGeometricMean(values), 2);

            Assert.AreEqual(expectedResult, result);
        }

        #endregion CalculationUtils Tests
    }
}
