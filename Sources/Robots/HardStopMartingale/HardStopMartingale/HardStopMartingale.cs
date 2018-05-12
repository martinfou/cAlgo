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
using System.Collections.Generic;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class HardStopMartingale : Robot
    {
        [Parameter("Initial Quantity (Lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double InitialQuantity { get; set; }

        [Parameter("Stop Loss", DefaultValue = 40)]
        public int StopLoss { get; set; }

        [Parameter("Take Profit", DefaultValue = 40)]
        public int TakeProfit { get; set; }


        [Parameter("Max loss in row", DefaultValue = 7)]
        public int MaxLossInRow { get; set; }

        private Random random = new Random();

        private Statistics statistics;

        protected override void OnStart()
        {
            Positions.Closed += OnPositionsClosed;

            statistics = new Statistics();
            statistics.lossInRow = 0;
            statistics.maxLossInRow = MaxLossInRow;
            statistics.lossArray = new int[20];
            ExecuteOrder(InitialQuantity, GetRandomTradeType());
        }

        protected override void OnStop()
        {
            for (int i = 1; i <= 10; i++)
            {
                Print(i + " " + statistics.lossArray[i]);
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
            Print("Closed");

            var position = args.Position;

            if (position.Label != "Martingale" || position.SymbolCode != Symbol.Code)
                return;

            if (position.GrossProfit > 0)
            {
                statistics.lossArray[statistics.lossInRow] = statistics.lossArray[statistics.lossInRow] + 1;
                statistics.lossInRow = 0;
                ExecuteOrder(InitialQuantity, GetRandomTradeType());
            }
            else
            {
                statistics.lossInRow = statistics.lossInRow + 1;
                ExecuteOrder(position.Quantity * 2, position.TradeType);
            }
        }

        private TradeType GetRandomTradeType()
        {
            return random.Next(2) == 0 ? TradeType.Buy : TradeType.Sell;
        }


        class Statistics
        {
            public int lossInRow { get; set; }
            public int maxLossInRow { get; set; }
            public int[] lossArray;
        }

    }
}
