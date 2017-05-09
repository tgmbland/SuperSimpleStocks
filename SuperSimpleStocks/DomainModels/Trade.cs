using System;

namespace SuperSimpleStocks.DomainModels
{
    public enum TradeTypeEnum
    {
        Buy,
        Sell
    }

    public struct Trade
    {
        public string StockSymbol { get; set; }
        public DateTime Timestamp { get; set; }
        public int Quantity { get; set; }
        public TradeTypeEnum TradeType { get; set; }
        public double TradePrice { get; set; }
    }
}
