using System;
using SuperSimpleStocks.DomainModels;
using SuperSimpleStocks.Services;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace SuperSimpleStocks.Tests
{
    public class ExchangeServiceTests
    {
        private ExchangeService _testee;

        #region ExchangeService Tests
        [SetUp]
        public void Setup()
        {
            _testee = new ExchangeService();
            _testee.AddStock(new CommonStockInfo("TEA", 0, 100));
            _testee.AddStock(new CommonStockInfo("POP", 8, 100));
            _testee.AddStock(new CommonStockInfo("ALE", 23, 60));
            _testee.AddStock(new PreferredStockInfo("GIN", 8, 100, 2));
            _testee.AddStock(new CommonStockInfo("JOE", 13, 250));
        }

        private void MakeSampleTrades()
        {
            _testee.MakeTrade("TEA", 5, TradeTypeEnum.Buy, 10);
            _testee.MakeTrade("POP", 10, TradeTypeEnum.Buy, 15);
            _testee.MakeTrade("GIN", 16, TradeTypeEnum.Sell, 100);
            _testee.MakeTrade("ALE", 102, TradeTypeEnum.Sell, 55);
            _testee.MakeTrade("TEA", 25, TradeTypeEnum.Sell, 60);
            _testee.MakeTrade("GIN", 65, TradeTypeEnum.Buy, 105);
            _testee.MakeTrade("ALE", 75, TradeTypeEnum.Buy, 57);
            _testee.MakeTrade("ALE", 56, TradeTypeEnum.Buy, 58);
            _testee.MakeTrade("JOE", 108, TradeTypeEnum.Sell, 45);
        }

        [Test]
        public void EnsureGetStockBySymbolWithInvalidSymbolThrowsException()
        {
            string stockSymbol = "DUD";
            string expectedMessage = "The stock you have specified: {stockSymbol} does not exist on this exchange.";

            Assert.Throws<InvalidOperationException>(() => _testee.GetStockBySymbol(stockSymbol), expectedMessage);
        }

        [Test]
        public void EnsureGetStockBySymbolWithValidSymbolReturnsStock()
        {
            string stockSymbol = "TEA";
            string expectedSymbol = "TEA";

            var result = _testee.GetStockBySymbol(stockSymbol);

            Assert.AreEqual(expectedSymbol, result.Symbol);
        }
        
        [Test]
        public void EnsureAttemptToAddExistingStockThrowsException()
        {
            BaseStockInfo stockInfo = new CommonStockInfo("ALE", 23, 60);
            string expectedMessage = $"The stock you have specified: {stockInfo.Symbol} has already been added to this exchange.\r\nParameter name: stock";
            Assert.Throws<ArgumentException>(() => _testee.AddStock(stockInfo), expectedMessage);            
        }

        [Test]
        public void EnsureAttemptToAddNewStockSucceeds()
        {
            BaseStockInfo stockInfo = new CommonStockInfo("VIN", 15, 85);
            int expectedStockCount = 6;
            _testee.AddStock(stockInfo);

            Assert.AreEqual(expectedStockCount, _testee.StockCount);
        }

        [Test]
        public void EnsureAttemptToMakeTradeWithInvalidStockSymbolThrowsException()
        {
            string stockSymbol = "DUD";
            string expectedMessage = "The stock you have specified: {stockSymbol} does not exist on this exchange.";

            Assert.Throws<InvalidOperationException>(() => _testee.MakeTrade(stockSymbol, 10, TradeTypeEnum.Buy, 10), expectedMessage);
        }

        [Test]
        public void EnsureAttemptToMakeTradeWithValidStockSymbolSucceeds()
        {
            int expectedTradeCount = 1;
            _testee.MakeTrade("TEA", 1500, TradeTypeEnum.Sell, 45.6);

            Assert.AreEqual(expectedTradeCount, _testee.TradeCount);            
        } 

        [Test]
        public void EnsureGetVolumeWeightedStockPriceForInvalidStockThrowsException()
        {
            string stockSymbol = "DUD";
            string expectedMessage = "The stock you have specified: {stockSymbol} does not exist on this exchange.";

            Assert.Throws<InvalidOperationException>(() => _testee.GetVolumeWeightedStockPrice(stockSymbol), expectedMessage);
        }

        [Test]
        public void EnsureGetVolumeWeightedStockPriceWhenNoTradesPresentThrowsException()
        {
            string stockSymbol = "TEA";
            string expectedMessage = "Unable to calculate Volume Weighted Stock Price as no trades found for {stockSymbol} in previous 15 minutes.";

            Assert.Throws<InvalidOperationException>(() => _testee.GetVolumeWeightedStockPrice(stockSymbol), expectedMessage);
        }

        [Test]
        [TestCase("TEA", 51.67)]
        [TestCase("ALE", 56.36)]
        [TestCase("GIN", 104.01)]
        public void EnsureGetVolumeWeightedStockPriceReturnsCorrectValue(string stockSymbol, double expectedResult)
        {
            MakeSampleTrades();
            var result = _testee.GetVolumeWeightedStockPrice(stockSymbol);

            Assert.AreEqual(expectedResult, Math.Round(result, 2));
        }

        [Test]
        public void EnsureGetAllShareIndexWhenNoMarketPriceSetThrowsException()
        {
            string stockSymbol = "ALE";
            string expectedMessage = $"The stock {stockSymbol} is listed as having a zero market price.";

            Assert.Throws<InvalidOperationException>(() => _testee.GetAllShareIndex(), expectedMessage);
        }
        
        [Test]
        public void EnsureGetAllShareIndexReturnsCorrectValue()
        {
            MakeSampleTrades();
            double expectedValue = 47.69;

            var result = _testee.GetAllShareIndex();

            Assert.AreEqual(expectedValue, Math.Round(result,2));
        }

        #endregion ExchangeService Tests
    }
}
