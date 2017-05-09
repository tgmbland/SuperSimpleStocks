namespace SuperSimpleStocks.DomainModels
{
    public class CommonStockInfo : BaseStockInfo
    {
        public CommonStockInfo(string symbol, double lastDividend, double parValue) : base(symbol, lastDividend, parValue)
        {
        }

        public override double DividendYield(double suppliedMarketPrice)
        {
            CheckSuppliedMarketPriceIsNotZero(suppliedMarketPrice);

            double divYield;
            divYield = _lastDividend / suppliedMarketPrice;
            return divYield;
        }
    }

}
