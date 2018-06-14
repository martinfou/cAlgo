// -------------------------------------------------------------------------------
//
//    This is a Template used as a guideline to build your own Robot. 
//    Please use the “Feedback” tab to provide us with your suggestions about cAlgo’s API.
//
// -------------------------------------------------------------------------------

using System;
using cAlgo.API;

namespace cAlgo.Robots
{
    [Robot()]
    public class NeoLan : Robot
    {

        [Parameter("Vol", DefaultValue = 100000)]
        public int Vol { get; set; }

        [Parameter("TP", DefaultValue = 100)]
        public int TP { get; set; }

        [Parameter("Exp", DefaultValue = 1.6)]
        public double Exp { get; set; }

        [Parameter("Step", DefaultValue = 500)]
        public double Step { get; set; }

        private int k1 = 0, k2 = 0;
        private Position position1, pos1;
        private long lot1, lot2, Lot;
        private double? TP1, TP2;
        private double take1, take2;
        private TradeType type;

        protected override void OnStart()
        {
            Print("Welcome to the world of infinite financial possibilities!");
        }

        protected override void OnTick()
        {

            foreach (var position in Account.Positions)
            {
                if (position.SymbolCode == Symbol.Code)
                {


                    double Bid = Symbol.Bid;
                    double Ask = Symbol.Ask;
                    double Point = Symbol.PointSize;

                    Lot = position.Volume;

                    double lot_buy = Vol * Math.Pow(Exp, k1 - 1);
                    double lot_sell = Vol * Math.Pow(Exp, k2 - 1);

                    double lot_b = Vol * Math.Pow(Exp, k1);
                    double lot_s = Vol * Math.Pow(Exp, k2);

                    int lb = ((int)lot_buy / 10000) * 10000;
                    int ls = ((int)lot_sell / 10000) * 10000;
                    int LB = ((int)lot_b / 10000) * 10000;
                    int LS = ((int)lot_s / 10000) * 10000;


                    if (k1 == 1)
                    {
                        lot1 = Vol;
                        TP1 = position.TakeProfit;
                    }
                    if (k2 == 1)
                    {
                        lot2 = Vol;
                        TP2 = position.TakeProfit;
                    }


                    if (Trade.IsExecuting)
                        return;

                    if (position.TradeType == TradeType.Buy && Lot == lb && position.EntryPrice - Ask >= Step * Point)
                    {
                        Trade.CreateBuyMarketOrder(Symbol, LB);
                        k1 = k1 + 1;
                        lot1 = lot1 + LB;
                        TP1 = (position.TakeProfit * (lot1 - LB) + Bid * LB) / lot1;
                        take1 = Math.Round((double)TP1, 5);
                    }

                    if (position.TradeType == TradeType.Sell && Lot == ls && Bid - position.EntryPrice >= Step * Point)
                    {
                        Trade.CreateSellMarketOrder(Symbol, LS);
                        k2 = k2 + 1;
                        lot2 = lot2 + LS;
                        TP2 = (position.TakeProfit * (lot2 - LS) + Bid * LS) / lot2;
                        take2 = Math.Round((double)TP2, 5);
                    }


                    if (position.TradeType == TradeType.Buy && take1 > 0 && k1 > 1)
                    {
                        if (take1 != position.TakeProfit)
                            Trade.ModifyPosition(position, position.StopLoss, take1);
                    }

                    if (position.TradeType == TradeType.Sell && take2 > 0 && k2 > 1)
                    {
                        if (take2 != position.TakeProfit)
                            Trade.ModifyPosition(position, position.StopLoss, take2);
                    }
                }
            }


            if (k1 == 0)
            {
                Trade.CreateBuyMarketOrder(Symbol, Vol);
                k1 = 1;
            }
            if (k2 == 0)
            {
                Trade.CreateSellMarketOrder(Symbol, Vol);
                k2 = 1;
            }
        }

        protected override void OnPositionOpened(Position openedPosition)
        {
            position1 = openedPosition;
            double Point = Symbol.PointSize;

            if (position1.TradeType == TradeType.Buy && k1 == 1)
            {
                Trade.ModifyPosition(openedPosition, position1.StopLoss, position1.EntryPrice + TP * Point);
            }

            if (position1.TradeType == TradeType.Sell && k2 == 1)
            {
                Trade.ModifyPosition(openedPosition, position1.StopLoss, position1.EntryPrice - TP * Point);
            }
        }

        protected override void OnPositionClosed(Position pos)
        {
            pos1 = pos;

            if (pos1.TradeType == TradeType.Buy)
                k1 = 0;

            if (pos1.TradeType == TradeType.Sell)
                k2 = 0;
        }

        protected override void OnStop()
        {
            Print("The FxPro company wishes you successful day!");
        }
    }
}
