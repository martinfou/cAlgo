// -------------------------------------------------------------------------------------------------
//
//    This code is a cAlgo API sample.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk
//
//    The "Sample Martingale cBot" creates a random Sell or Buy order. If the Stop loss is hit, a new 
//    order of the same type (Buy / Sell) is created with double the Initial Volume amount. The cBot will 
//    continue to double the volume amount for  all orders created until one of them hits the take Profit. 
//    After a Take Profit is hit, a new random Buy or Sell order is created with the Initial Volume amount.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleMartingalecBot : Robot
    {
        [Parameter("Initial Quantity (Lots)", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double InitialQuantity { get; set; }

        [Parameter("Stop Loss", DefaultValue = 40)]
        public int StopLoss { get; set; }

        [Parameter("Take Profit", DefaultValue = 40)]
        public int TakeProfit { get; set; }

        [Parameter("Max Loss In a Day", DefaultValue = 3, MinValue = 1, Step = 1)]
        public int MaxLossInDay { get; set; }

        private Random random = new Random();
        private static int today = 0;
        private bool isAllowedToTrade = false;
        protected override void OnStart()
        {
            Positions.Closed += OnPositionsClosed;
            today = Server.Time.Day;
            isAllowedToTrade = true;
            ExecuteOrder(InitialQuantity, GetRandomTradeType());
        }

        protected override void OnBar()
        {
            Print("# position open " + Positions.Count);
            if (today != Server.Time.Day)
            {
                isAllowedToTrade = true;
                today = Server.Time.Day;
            }

            if (Positions.Count == 0)
            {
                Print("no position open");
            }

        }

        private void ExecuteOrder(double quantity, TradeType tradeType)
        {
            var volumeInUnits = Symbol.QuantityToVolume(quantity);
            var result = ExecuteMarketOrder(tradeType, Symbol, volumeInUnits, "Martingale", StopLoss, TakeProfit);

            if (result.Error == ErrorCode.NoMoney)
                Stop();
        }

        private void OnPositionsClosed(PositionClosedEventArgs args)
        {
            Print("Closed " + Server.Time.Day + " " + isAllowedToTrade);
            var position = args.Position;
            if (isAllowedToTrade)
            {
                if (position.Label != "Martingale" || position.SymbolCode != Symbol.Code)
                    return;

                if (position.GrossProfit > 0)
                {
                    ExecuteOrder(InitialQuantity, GetRandomTradeType());
                }
                else
                {
                    isAllowedToTrade = false;
                    ExecuteOrder(position.Quantity * 2, position.TradeType);
                }
            }
        }

        private TradeType GetRandomTradeType()
        {
            return random.Next(2) == 0 ? TradeType.Buy : TradeType.Sell;
        }
    }
}
