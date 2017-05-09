using System;
using System.Collections.Generic;
using System.Linq;
using SuperSimpleStocks.DomainModels;
using SuperSimpleStocks.Utils;

namespace SuperSimpleStocks.Services
{
    public class ExchangeService
    {
        private List<BaseStockInfo> _stocks;
        private List<Trade> _trades;

        public int StockCount
        {
            get { return _stocks.Count; }
        }

        public int TradeCount
        {
            get { return _trades.Count; }
        }

        public ExchangeService()
        {
            _stocks = new List<BaseStockInfo>();
            _trades = new List<Trade>();
        }

        public BaseStockInfo GetStockBySymbol(string symbol)
        {
            ThrowExIfStockDoesNotExist(symbol);
            return _stocks.First(si => si.Symbol.ToUpper() == symbol.ToUpper());
        }

        public void AddStock(BaseStockInfo stock)
        {
            if (CheckIfStockExists(stock.Symbol))
                throw new ArgumentException($"The stock you have specified: {stock.Symbol} has already been added to this exchange.", "stock");

            _stocks.Add(stock);
        }

        public void MakeTrade(string stockSymbol, int quantity, TradeTypeEnum tradeType, double tradePrice)
        {
            ThrowExIfStockDoesNotExist(stockSymbol);

            _trades.Add(new Trade { Timestamp = DateTime.Now, StockSymbol = stockSymbol, Quantity = quantity, TradePrice = tradePrice, TradeType = tradeType });
            _stocks.First(si => si.Symbol.ToUpper() == stockSymbol.ToUpper()).MarketPrice = tradePrice;
        }

        public double GetVolumeWeightedStockPrice(string stockSymbol)
        {
            ThrowExIfStockDoesNotExist(stockSymbol);

            int previousMinutes = 15;

            List<Trade> selectedTrades = GetSelectedTrades(stockSymbol, previousMinutes);
            if (selectedTrades.Count == 0)
                throw new InvalidOperationException("Unable to calculate Volume Weighted Stock Price as no trades found for {stockSymbol} in previous {previousMinutes} minutes.");

            double volWeightedStockPrice = 0;

            double enumerator = selectedTrades.Sum(tr => tr.TradePrice * tr.Quantity);
            double denominator = selectedTrades.Sum(tr => tr.Quantity);

            volWeightedStockPrice = enumerator / denominator;

            return volWeightedStockPrice;
        }

        public double GetAllShareIndex()
        {
            double allShareIndex = 0;

            ThrowExIfAnyStockHasZeroMarketPrice();

            double[] marketValues = _stocks.Select(s => s.MarketPrice).ToArray();         

            allShareIndex = CalculationUtils.CalculateGeometricMean(marketValues);

            return allShareIndex;
        }

        #region Private methods

        private void ThrowExIfStockDoesNotExist(string stockSymbol)
        {
            if (!CheckIfStockExists(stockSymbol))
                throw new InvalidOperationException($"The stock you have specified: {stockSymbol} does not exist on this exchange.");
        }

        private void ThrowExIfAnyStockHasZeroMarketPrice()
        {
            BaseStockInfo stockInfo = _stocks.OrderBy(si => si.Symbol).FirstOrDefault(si => si.MarketPrice == 0);
            if (stockInfo != null)
                throw new InvalidOperationException($"The stock {stockInfo.Symbol} is listed as having a zero market price.");
        }

        private bool CheckIfStockExists(string stockSymbol)
        {
            BaseStockInfo foundStock = _stocks.FirstOrDefault(si => si.Symbol.ToUpper() == stockSymbol.ToUpper());
            if (foundStock == null)
            {
                return false;
            }
            return true;
        }

        private List<Trade> GetSelectedTrades(string stockSymbol, int minutesPrevious)
        {
            return _trades.Where(tr => tr.StockSymbol.ToUpper() == stockSymbol.ToUpper() &&
            tr.Timestamp > DateTime.Now.AddMinutes(-minutesPrevious)).ToList();
        }

        #endregion Private methods
    }
}
