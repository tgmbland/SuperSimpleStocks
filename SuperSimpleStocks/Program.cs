using System;
using SuperSimpleStocks.Services;
using SuperSimpleStocks.DomainModels;
using SuperSimpleStocks.ExtensionMethods;

namespace SuperSimpleStocks
{
    class Program
    {
        private static ExchangeService exchange;

        static void Main(string[] args)
        {
            exchange = InitExchange();
            DisplayHelp();
            DisplayPrompt();

            bool quit = false;
            while(!quit)
            {
                string command = Console.ReadLine();
                InterpretCommand(command, out quit);
                if (!quit)
                    DisplayPrompt();
            }                         
        }        

        private static ExchangeService InitExchange()
        {
            ExchangeService exchange;
            exchange = new ExchangeService();
            exchange.AddStock(new CommonStockInfo("TEA", 0, 100));
            exchange.AddStock(new CommonStockInfo("POP", 8, 100));
            exchange.AddStock(new CommonStockInfo("ALE", 23, 60));
            exchange.AddStock(new PreferredStockInfo("GIN", 8, 100, 2));
            exchange.AddStock(new CommonStockInfo("JOE", 13, 250));

            return exchange;
        }

        #region Command Parsing/Execution methods

        private static void InterpretCommand(string commandString, out bool quitNow)
        {            
            if (commandString.Trim().ToUpper() == "QUIT")
            { 
                quitNow = true;
                return;
            }
            string[] commandArgs = commandString.Split(' ');
            if (commandArgs.Length == 0)
                DisplayInvalidCommandMessage();
            else
                switch (GetInitialCommand(commandArgs))
                {
                    case "QUIT":
                        quitNow = true;
                        return;
                    case "HELP":
                        DisplayHelp();
                        break;
                    case "DY":
                        DoDYorPECommand(commandArgs);
                        break;
                    case "PE":
                        DoDYorPECommand(commandArgs);
                        break;
                    case "MT":
                        DoMTCommand(commandArgs);
                        break;
                    case "INDEX":
                        DoIndexCommand();
                        break;
                    case "VWSP":
                        DoVWSPCommand(commandArgs);
                        break;
                    default:
                        DisplayInvalidCommandMessage();
                        break;
                }

            quitNow = false;
        }

        private static void DoDYorPECommand(string[] commandArgs)
        {
            if (commandArgs.Length == 3)
            {
                string symbol = GetSymbolFromCommandArgs(commandArgs);
                double marketPrice; 
                if(double.TryParse(commandArgs[2], out marketPrice))
                {
                    try
                    {
                        if (GetInitialCommand(commandArgs) == "DY")
                        {
                            var result = exchange.GetStockBySymbol(symbol).DividendYield(marketPrice);
                            Console.WriteLine($"Dividend Yield for {symbol} at price of {marketPrice} is: {result}.");
                        }
                        else
                        {
                            var result = exchange.GetStockBySymbol(symbol).PriceEarningsRatio(marketPrice);
                            Console.WriteLine($"P/E Ratio for {symbol} at price of {marketPrice} is: {result}.");
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        DisplayExceptionMessage(ex);
                    }
                    return;
                }
            }
            DisplayInvalidCommandMessage();
        }

        private static void DoMTCommand(string[] commandArgs)
        {
            if (commandArgs.Length == 5)
            {
                string symbol = GetSymbolFromCommandArgs(commandArgs);
                int quantity;
                if (int.TryParse(commandArgs[2], out quantity))
                {
                    TradeTypeEnum operation;
                    if (Enum.TryParse<TradeTypeEnum>(commandArgs[3].Trim().ToTitleCase(), out operation))
                    {
                        double price;
                        if(double.TryParse(commandArgs[4], out price))
                        {
                            try
                            {
                                exchange.MakeTrade(symbol, quantity, operation, price);
                                Console.WriteLine("Trade Successful");
                            }
                            catch(Exception ex)
                            {
                                DisplayExceptionMessage(ex);
                            }
                            return;
                        }
                    }
                }
            }
            DisplayInvalidCommandMessage();
        }

        private static void DoIndexCommand()
        {
            try
            {
                var result = exchange.GetAllShareIndex();
                Console.WriteLine($"GBCE All Share Index: {result}");
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }

        private static void DoVWSPCommand(string[] commandArgs)
        {
            if(commandArgs.Length == 2)
            {
                try
                {
                    string symbol = GetSymbolFromCommandArgs(commandArgs);
                    double result = exchange.GetVolumeWeightedStockPrice(symbol);
                    Console.WriteLine($"The VWSP for {symbol} is {result}.");
                }
                catch(Exception ex)
                {
                    DisplayExceptionMessage(ex);
                }
                return;
            }
            DisplayInvalidCommandMessage();
        }        

        private static string GetInitialCommand(string[] commandArgs)
        {
            return commandArgs[0].Trim().ToUpper();
        }

        private static string GetSymbolFromCommandArgs(string[] commandArgs)
        {
            return commandArgs[1].Trim();
        }

        #endregion Command Parsing/Execution methods

        #region Display Methods

        private static void DisplayHelp()
        {
            Console.WriteLine("****************** SuperSimpleStocks ********************");
            Console.WriteLine("*                                                       *");
            Console.WriteLine("*                   Commands:                           *");
            Console.WriteLine("* 1) Show this screen -> HELP                           *");
            Console.WriteLine("* 2) Get Dividend Yield -> DY <Symbol> <MarketPrice>    *");
            Console.WriteLine("*   e.g. >DY TEA 35                                     *");
            Console.WriteLine("* 3) Get P/E Ratio -> PE <Symbol> <MarketPrice>         *");
            Console.WriteLine("*   e.g. >PE TEA 35                                     *");
            Console.WriteLine("* 4) Make Trade -> MT <Symbol> <Amt> <Buy/Sell> <Price> *");
            Console.WriteLine("*   e.g. >MT TEA 102 Buy 55                             *");
            Console.WriteLine("* 5) Get Vol. Weighted Stock Price -> VWSP <Symbol>     *");
            Console.WriteLine("*   e.g. >VWSP TEA                                      *");
            Console.WriteLine("* 6) Get GBCE All Share Index -> INDEX                  *");
            Console.WriteLine("* 7) Quit -> QUIT                                       *");
            Console.WriteLine("*                                                       *");
            Console.WriteLine("*********************************************************");
        }

        private static void DisplayPrompt()
        {
            Console.Write(">");
        }

        private static void DisplayInvalidCommandMessage()
        {
            Console.WriteLine("Invalid Command");
        }

        private static void DisplayExceptionMessage(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        #endregion Display Methods
    }
}



  