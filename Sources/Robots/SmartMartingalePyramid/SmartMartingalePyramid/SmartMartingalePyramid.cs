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
    public class SmartMartingalePyramid : Robot
    {
        [Parameter("Initial Quantity (Lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double InitialQuantity { get; set; }

        [Parameter("Max Loss In A Row", DefaultValue = 6, MinValue = 1, Step = 1)]
        public int MaxLossInRow { get; set; }

        [Parameter("Stop Loss", DefaultValue = 40)]
        public int StopLoss { get; set; }

        [Parameter("Take Profit", DefaultValue = 40)]
        public int TakeProfit { get; set; }

        private Random random = new Random();
        private int lossInRow = 0;
        private double level1bet = 0;
        private int SIMPLE_SPEED = 1;
        private int DOUBLE_SPEED = 2;
        private int QUATRUPLE_SPEED = 4;
        private int curentSpeedLevel = 1;
        private double maxAccountBalance = 0;
        private int currentMaxLossInRow = 0;

        protected override void OnStart()
        {
            Positions.Closed += OnPositionsClosed;
            level1bet = InitialQuantity;
            maxAccountBalance = Account.Balance;
            currentMaxLossInRow = MaxLossInRow;
            ExecuteOrder(InitialQuantity, GetRandomTradeType());
        }

        private void ExecuteOrder(double quantity, TradeType tradeType)
        {
            var volumeInUnits = Symbol.QuantityToVolume(quantity);
            var result = ExecuteMarketOrder(tradeType, Symbol, volumeInUnits, "smartmartingale", StopLoss, TakeProfit);

            if (result.Error == ErrorCode.NoMoney)
                Stop();
        }

        private void OnPositionsClosed(PositionClosedEventArgs args)
        {
            var position = args.Position;

            Print("Quantity " + position.Quantity + " MaxLossInRow " + MaxLossInRow + " lossInRow " + lossInRow + " level1Bet " + level1bet + " Account Balance " + Account.Balance);

            if (position.Label != "smartmartingale" || position.SymbolCode != Symbol.Code)
                return;

            if (position.GrossProfit > 0)
            {
                lossInRow = 0;
                currentMaxLossInRow = MaxLossInRow;
                maxAccountBalance = Account.Balance;
                ExecuteOrder(level1bet, GetRandomTradeType());
            }
            else
            {
                lossInRow = lossInRow + 1;

                if (lossInRow >= currentMaxLossInRow)
                {
                    if (currentMaxLossInRow <= 4)
                    {
                        Stop();
                    }

                    level1bet = level1bet * 2;
                    currentMaxLossInRow = currentMaxLossInRow - 1;
                    ExecuteOrder(level1bet, position.TradeType);
                    return;
                }
                else
                {
                    if (Account.Balance > 5000)
                    {
                        currentMaxLossInRow = MaxLossInRow;
                        level1bet = InitialQuantity;
                        ExecuteOrder(level1bet, position.TradeType);
                        return;
                    }
                    else
                    {
                        ExecuteOrder(position.Quantity * 2, position.TradeType);
                    }
                }
            }
        }

        private TradeType GetRandomTradeType()
        {
            return random.Next(2) == 0 ? TradeType.Buy : TradeType.Sell;
        }

    }
}
