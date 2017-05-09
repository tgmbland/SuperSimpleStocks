using System;

namespace SuperSimpleStocks.DomainModels
{
    public abstract class BaseStockInfo
    {
        protected string _symbol;
        protected double _lastDividend;
        protected double _parValue;

        protected double _marketPrice;

        public string Symbol { get { return _symbol; } }

        public BaseStockInfo(string symbol, double lastDividend, double parValue)
        {
            _symbol = symbol;
            _lastDividend = lastDividend;
            _parValue = parValue;
        }

        internal double MarketPrice
        {
            get { return _marketPrice; }
            set { _marketPrice = value; }
        }

        public abstract double DividendYield(double suppliedMarketPrice);

        public double PriceEarningsRatio(double suppliedMarketPrice)
        {
            CheckSuppliedMarketPriceIsNotZero(suppliedMarketPrice);
            if (_lastDividend == 0)
            {
                throw new InvalidOperationException($"Cannot calculate PERatio for {_symbol} as lastDividend value is zero.");
            }

            double peRatio;
            peRatio = suppliedMarketPrice / _lastDividend;
            return peRatio;
        }

        protected void CheckSuppliedMarketPriceIsNotZero(double suppliedMarketPrice)
        {
            if (suppliedMarketPrice == 0)
            {
                throw new ArgumentException("The value supplied is invalid. Please supply a number greater than zero.", "suppliedMarketPrice");
            }
        }
    }
}
