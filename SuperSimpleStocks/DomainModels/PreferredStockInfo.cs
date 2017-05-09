namespace SuperSimpleStocks.DomainModels
{
    public class PreferredStockInfo : BaseStockInfo
    {
        protected double _fixedDividendPercentage;

        public PreferredStockInfo(string symbol, double lastDividend, double parValue, double fixedDividendPercentage) : base(symbol, lastDividend, parValue)
        {
            _fixedDividendPercentage = fixedDividendPercentage;
        }

        public override double DividendYield(double suppliedMarketPrice)
        {
            CheckSuppliedMarketPriceIsNotZero(suppliedMarketPrice);

            double divYield;
            divYield = (_fixedDividendPercentage * 0.01 * _parValue) / suppliedMarketPrice;
            return divYield;
        }
    }

}
