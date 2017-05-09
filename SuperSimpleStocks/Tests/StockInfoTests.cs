using System;
using SuperSimpleStocks.DomainModels;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace SuperSimpleStocks.Tests
{
    [TestFixture]
    public class StockInfoTests
    {
        #region StockInfo Tests

        [Test]
        [TestCase("Common")]
        [TestCase("Preferred")]
        public void EnsureAttemptToCalculateDividendYieldWithZeroMarketPriceThrowsException(string stockType)
        {
            BaseStockInfo stockInfo;
            if (stockType == "Common")
            {
                stockInfo = new CommonStockInfo("ALE", 23, 60);
            }
            else
            {
                stockInfo = new PreferredStockInfo("GIN", 8, 100, 2);
            }

            var expectedParamName = "suppliedMarketPrice";
            var expectedMessage = $"The value supplied is invalid. Please supply a number greater than zero.\r\nParameter name: {expectedParamName}";

            Assert.Throws<ArgumentException>(() => stockInfo.DividendYield(0), expectedMessage);
        }

        [Test]
        public void EnsureAttemptToCalculatePERatioWithZeroDividendThrowsException()
        {
            BaseStockInfo stockInfo = new CommonStockInfo("TEA", 0, 100);
            var expectedMessage = "Cannot calculate PERatio for TEA as lastDividend value is zero.";

            Assert.Throws<InvalidOperationException>(() => stockInfo.PriceEarningsRatio(10), expectedMessage);
        }

        [Test]
        [TestCase("TEA", 0, 100, 10, 0)]
        [TestCase("POP", 8, 100, 10, 0.8)]
        [TestCase("ALE", 23, 60, 23, 1)]
        [TestCase("JOE", 13, 250, 2, 6.5)]
        public void EnsureDividendYieldIsCorrectForCommonStock(string symbol, double lastDividend, double parValue, double marketPrice, double expectedDivYield)
        {
            CommonStockInfo commonStock = new CommonStockInfo(symbol, lastDividend, parValue);

            var result = commonStock.DividendYield(marketPrice);

            Assert.AreEqual(expectedDivYield, result);
        }

        [Test]
        [TestCase("POP", 8, 100, 10, 1.25)]
        [TestCase("ALE", 23, 60, 23, 1)]
        [TestCase("JOE", 13, 250, 2, 1.0 / 6.5)]
        public void EnsurePriceEarningRatioIsCorrect(string symbol, double lastDividend, double parValue, double marketPrice, double expectedPERatio)
        {
            CommonStockInfo commonStock = new CommonStockInfo(symbol, lastDividend, parValue);

            var result = commonStock.PriceEarningsRatio(marketPrice);

            Assert.AreEqual(expectedPERatio, result);
        }

        [Test]
        public void EnsureDividendYieldIsCorrectForPreferredStock()
        {
            PreferredStockInfo preferredStock = new PreferredStockInfo("GIN", 8, 100, 2);
            double expectedYield = 1.0;

            var result = preferredStock.DividendYield(2);

            Assert.AreEqual(expectedYield, result);
        }

        #endregion StockInfo Tests

    }
}
