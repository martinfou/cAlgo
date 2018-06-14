// -------------------------------------------------------------------------------
//
//    This is a Template used as a guideline to build your own Robot. 
//    Please use the “Feedback” tab to provide us with your suggestions about cAlgo’s API.
//
// -------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    [Robot()]
    public class BARS_3 : Robot
    {
        [Parameter("Vol", DefaultValue = 10000)]
        public int Vol { get; set; }
        [Parameter("SL", DefaultValue = 100)]
        public int SL { get; set; }
        [Parameter("TP", DefaultValue = 100)]
        public int TP { get; set; }
        private DataSeries Price { get; set; }
        private Position op_pos;
        protected override void OnStart()
        {
            // Put your initialization logic here
        }
        private void verify_3bars_and_trade()
        {
            int bars = MarketSeries.Close.Count - 1;
            int[] b_or_m = new int[3];
            Print("{0},{1},{2},{3},{4}", MarketSeries.High[bars - 1], MarketSeries.High[bars - 2], MarketSeries.High[bars - 3], MarketSeries.Close.Count, MarketSeries.High[1]);
            if (MarketSeries.Close[bars - 1] > MarketSeries.Open[bars - 1])
                b_or_m[0] = 1;
            else
                //бычья
                b_or_m[0] = 2;
            //медвежья
            if (MarketSeries.Close[bars - 2] > MarketSeries.Open[bars - 2])
                b_or_m[1] = 1;
            else
                //бычья
                b_or_m[1] = 2;
            //медвежья
            if (MarketSeries.Close[bars - 3] > MarketSeries.Open[bars - 3])
                b_or_m[2] = 1;
            else
                //бычья
                b_or_m[2] = 2;
            //медвежья
            Print("{0},{1},{2}", b_or_m[0], b_or_m[1], b_or_m[2]);
            TradeType op = TradeType.Buy;
            int b_or_s = 0;
            if ((b_or_m[0] == 1) && (b_or_m[1] == 1) && (b_or_m[2] == 1))
            {
                op = TradeType.Buy;
                b_or_s = 1;
            }
            if ((b_or_m[0] == 2) && (b_or_m[1] == 2) && (b_or_m[2] == 2))
            {
                op = TradeType.Sell;
                b_or_s = 2;
            }
            if (b_or_s != 0)
            {
                Trade.CreateMarketOrder(op, Symbol, Vol);
                Print("{0} {1} with vol={2}", Symbol.Code, op, Vol);
            }

        }
        protected override void OnTick()
        {
        }
        protected override void OnBar()
        {
            if (op_pos == null)
                verify_3bars_and_trade();
        }

        protected override void OnPositionOpened(Position pos)
        {
            double SL_pr = 0;
            double TP_pr = 0;
            op_pos = pos;
            if (pos.TradeType == TradeType.Sell)
            {
                SL_pr = pos.EntryPrice + SL * Symbol.PipSize;
                TP_pr = pos.EntryPrice - TP * Symbol.PipSize;
            }
            if (pos.TradeType == TradeType.Buy)
            {
                SL_pr = pos.EntryPrice - SL * Symbol.PipSize;
                TP_pr = pos.EntryPrice + TP * Symbol.PipSize;
            }
            Trade.ModifyPosition(pos, SL_pr, TP_pr);
            Print("Position {0} modify, SL={1}, TP={2}", pos.Id, SL_pr, TP_pr);


        }

        protected override void OnPositionClosed(Position pos)
        {
            op_pos = null;
        }
        protected override void OnError(Error error)
        {
            Print("Errot={0}", error.Code);
        }
        protected override void OnStop()
        {
            Print("Robot 3BARS by VovkaSOL stopped by user");
        }
    }
}
