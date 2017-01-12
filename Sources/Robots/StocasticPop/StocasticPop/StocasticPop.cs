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
    public class StocasticPop : Robot
    {
        [Parameter("Initial Quantity (Lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double InitialQuantity { get; set; }

        [Parameter("Stochatic oversold", DefaultValue = 70, MinValue = 60, Step = 1)]
        public double StochasticOversold { get; set; }

        [Parameter("Stochatic undersold", DefaultValue = 30, MinValue = 10, Step = 1)]
        public double StochasticUndersold { get; set; }

        [Parameter("Line Crossing Range", DefaultValue = 0, MinValue = 0, Step = 1)]
        public int LINE_CROSSING_RANGE { get; set; }

        private double initialDeposit = 100;
        private StochasticOscillator stochastic;
        private double PositionSize;
        private double startOfDayAccountBalance;
        private double endOfDayAccountBalance;

        enum WhatToDo
        {
            Buy,
            Sell,
            Close,
            Nothing
        }

        protected override void OnStart()
        {
            initialDeposit = Account.Balance;
            stochastic = Indicators.StochasticOscillator(9, 3, 9, MovingAverageType.Simple);
            PositionSize = InitialQuantity;
        }

        protected override void OnBar()
        {
            StartOfDay();
            EndOfDay();

            if (getTradeType().Equals(WhatToDo.Close))
            {
                CloseOrder();
            }

            if (Positions.Count < 1)
            {

                if (getTradeType().Equals(WhatToDo.Buy))
                {
                    ExecuteOrder(PositionSize, TradeType.Buy);
                }
                if (getTradeType().Equals(WhatToDo.Sell))
                {
                    ExecuteOrder(PositionSize, TradeType.Sell);
                }
            }

        }

        private void ExecuteOrder(double quantity, TradeType tradeType)
        {
            var volumeInUnits = Symbol.QuantityToVolume(quantity);
            var result = ExecuteMarketOrder(tradeType, Symbol, volumeInUnits, "stochasticPop");

            if (result.Error == ErrorCode.NoMoney)
                Stop();
        }

        private void CloseOrder()
        {
            var position = Positions.Find("stochasticPop");
            if (position != null)
            {
                ClosePosition(position);
            }

        }

        private void EndOfDay()
        {
            endOfDayAccountBalance = this.Account.Balance;

            if (Server.Time.Hour >= 23 && Server.Time.Minute >= 0)
            {
                CloseOrder();
                Print("Profits for today = ", endOfDayAccountBalance - startOfDayAccountBalance);
            }

        }

        private void StartOfDay()
        {

            if ((Server.Time.Hour == 0) && (Server.Time.Minute >= 0))
            {
                startOfDayAccountBalance = this.Account.Balance;
            }

        }

        private WhatToDo getTradeType()
        {
            if (stochastic.PercentK.LastValue >= 70)
            {
                return WhatToDo.Buy;
            }
            else if (stochastic.PercentK.LastValue <= 30)
            {
                return WhatToDo.Sell;
            }
            else if (Functions.HasCrossedAbove(stochastic.PercentK, stochastic.PercentD, LINE_CROSSING_RANGE))
            {
                return WhatToDo.Close;
            }
            else if (Functions.HasCrossedBelow(stochastic.PercentK, stochastic.PercentD, LINE_CROSSING_RANGE))
            {
                return WhatToDo.Close;
            }
            else
            {
                return WhatToDo.Nothing;
            }

        }
    }
}
