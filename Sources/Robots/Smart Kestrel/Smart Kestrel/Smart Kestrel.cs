//+------------------------------------------------------------------+
//|                                             Smart Kestrel.cBot   |
//|                                             Version 7.4          |
//|                                         Copyright 2016, MD SAIF. |
//|                                  https://www.facebook.com/cls.fx |
//+------------------------------------------------------------------+
using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Internals;
namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SmartKestrel : Robot
    {
        [Parameter("Safety Limit", DefaultValue = 0.0)]
        public double SafetyLimit { get; set; }
        [Parameter("Max Spread", DefaultValue = 3.0)]
        public double MaxSpread { get; set; }
        [Parameter("Risk Per Trade", DefaultValue = 0.0)]
        public double RiskPerTrade { get; set; }
        [Parameter("Lot Size", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double LotSize { get; set; }
        [Parameter("Bars Count", DefaultValue = 12, MinValue = 1)]
        public int BarsCount { get; set; }
        [Parameter("Hour To Hour", DefaultValue = false)]
        public bool HourToHour { get; set; }
        [Parameter("Start Hour", DefaultValue = 8, MaxValue = 23)]
        public int StartHour { get; set; }
        [Parameter("Stop Hour", DefaultValue = 18, MaxValue = 23)]
        public int StopHour { get; set; }
        [Parameter("Primary SL", DefaultValue = 20.0)]
        public double PrimarySL { get; set; }
        [Parameter("Reversal SL", DefaultValue = true)]
        public bool ReversalSL { get; set; }
        [Parameter("Reversal Trailing", DefaultValue = true)]
        public bool ReversalTrailing { get; set; }
        [Parameter("Calculate By Pip", DefaultValue = true)]
        public bool CalculateByPip { get; set; }
        private string Label = "kestrel";
        private bool st_45 = true;
        private double highest = 0.0;
        private double lowest = 0.0;
        private double spread = 0.0;
        private double pnt_3;
        private string stop_case = "";
        private bool c_hour = true;
        private double primary_sl = 0.0;
        private bool safety_limit;
        //+------------------------------------------------------------------+
        //| cBot initialization function                                     |
        //+------------------------------------------------------------------+
        protected override void OnStart()
        {
            if (CalculateByPip)
            {
                pnt_3 = Symbol.PipSize;
                primary_sl = PrimarySL;
            }
            else
            {
                pnt_3 = Symbol.TickSize;
                if (Symbol.Digits % 2 == 1)
                    primary_sl = Math.Round(PrimarySL / 10.0, 1);
                else
                    primary_sl = PrimarySL;
            }
        }
        //+------------------------------------------------------------------+
        //| cBot tick function                                               |
        //+------------------------------------------------------------------+
        protected override void OnTick()
        {
            spread = Sz(Symbol.Ask, Symbol.Bid);
            safety_limit = Account.Balance > SafetyLimit && Account.Equity > SafetyLimit;
            RobotEngine();
            c_hour = GetTradingHour();
            if (spread <= MaxSpread && c_hour && st_45 && safety_limit)
            {
                var AllPos = Positions.FindAll(Label, Symbol);
                if (AllPos.Length == 0)
                    SendOrder();
            }
            var BuyPos = Positions.Find(Label, Symbol, TradeType.Buy);
            if (BuyPos != null)
            {
                if (PrimarySL > 0.0)
                    BuyManager(2);
                if (ReversalSL || ReversalTrailing)
                    BuyManager(3);
            }
            var SellPos = Positions.Find(Label, Symbol, TradeType.Sell);
            if (SellPos != null)
            {
                if (PrimarySL > 0.0)
                    SellManager(2);
                if (ReversalSL || ReversalTrailing)
                    SellManager(3);
            }
            string ld_10 = ChartComment();
            ChartObjects.DrawText("pan", ld_10, StaticPosition.TopLeft, Colors.DeepPink);
        }
        //+------------------------------------------------------------------+
        //| cBot Error Function                                              |
        //+------------------------------------------------------------------+
        protected override void OnError(Error error)
        {
            if (error.Code == ErrorCode.NoMoney)
            {
                Print("Order opening stopped because: not enough money");
                stop_case = "Not enough money";
                st_45 = false;
            }
        }
        //+------------------------------------------------------------------+
        //| cBot deinitialization function                                   |
        //+------------------------------------------------------------------+
        protected override void OnStop()
        {
            ChartObjects.RemoveAllObjects();
        }
        //+------------------------------------------------------------------+
        //| Setup highest and lowest price of counted bars                   |
        //+------------------------------------------------------------------+
        private void RobotEngine()
        {
            if (MarketSeries.Close.Count - 5 > BarsCount)
            {
                highest = MarketSeries.High.Maximum(BarsCount);
                lowest = MarketSeries.Low.Minimum(BarsCount);
                ChartObjects.DrawLine("hln", MarketSeries.OpenTime.Last(BarsCount), highest, MarketSeries.OpenTime.LastValue, highest, Colors.DeepPink, 1, LineStyle.Solid);
                ChartObjects.DrawLine("lln", MarketSeries.OpenTime.Last(BarsCount), lowest, MarketSeries.OpenTime.LastValue, lowest, Colors.DeepPink, 1, LineStyle.Solid);
            }
            else
                Print("Not enough bars loaded for bar_count function");
        }
        //+------------------------------------------------------------------+
        //| Trade setup function                                             |
        //+------------------------------------------------------------------+
        private void SendOrder()
        {
            bool sell_eng = lowest > 0.0 && Symbol.Bid <= lowest;
            bool buy_eng = highest > 0.0 && Symbol.Bid >= highest;
            if (sell_eng)
            {
                if (!OrderSend(TradeType.Sell, primary_sl).IsSuccessful)
                    Print("Sell opening error at: ", Symbol.Bid, ", ", LastResult.Error);
            }
            else if (buy_eng)
            {
                if (!OrderSend(TradeType.Buy, primary_sl).IsSuccessful)
                    Print("Buy opening error at: ", Symbol.Ask, ", ", LastResult.Error);
            }
        }
        //+------------------------------------------------------------------+
        //| Order send command                                               |
        //+------------------------------------------------------------------+
        private TradeResult OrderSend(TradeType type, double sl)
        {
            long ivol = VolumeSetup();
            return (ExecuteMarketOrder(type, Symbol, ivol, Label, sl, 0));
        }
        //+------------------------------------------------------------------+
        //| SLTP management of buy positions                                 |
        //+------------------------------------------------------------------+
        private void BuyManager(int id_m)
        {
            foreach (var position in Positions)
            {
                if (position.Label == Label && position.SymbolCode == Symbol.Code)
                {
                    if (position.TradeType == TradeType.Buy)
                    {
                        switch (id_m)
                        {
                            case 2:
                                {
                                    double li_16 = Nd(position.EntryPrice - PrimarySL * pnt_3);
                                    if (Symbol.Bid > li_16)
                                    {
                                        if (position.StopLoss < li_16 || position.StopLoss == null)
                                        {
                                            if (!ModifyPosition(position, li_16, position.TakeProfit).IsSuccessful)
                                                Print("Buy 'Primary SL' error at: ", li_16, ", ", LastResult.Error);
                                        }
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    double li_16 = Nd(lowest);
                                    if (Symbol.Bid > li_16)
                                    {
                                        if (position.StopLoss < li_16 || position.StopLoss == null)
                                        {
                                            if (!ModifyPosition(position, li_16, position.TakeProfit).IsSuccessful)
                                                Print("Buy 'Reversal SL/Reversal Trailing' error at: ", li_16, ", ", LastResult.Error);
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }
        //+------------------------------------------------------------------+
        //| SLTP management of sell positions                                |
        //+------------------------------------------------------------------+
        private void SellManager(int id_m)
        {
            foreach (var position in Positions)
            {
                if (position.Label == Label && position.SymbolCode == Symbol.Code)
                {
                    if (position.TradeType == TradeType.Sell)
                    {
                        switch (id_m)
                        {
                            case 2:
                                {
                                    double li_16 = Nd(position.EntryPrice + PrimarySL * pnt_3);
                                    if (Symbol.Ask < li_16)
                                    {
                                        if (position.StopLoss > li_16 || position.StopLoss == null)
                                        {
                                            if (!ModifyPosition(position, li_16, position.TakeProfit).IsSuccessful)
                                                Print("Sell 'Primary SL' error at: ", li_16, ", ", LastResult.Error);
                                        }
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    double li_16 = Nd(highest);
                                    if (Symbol.Ask < li_16)
                                    {
                                        if (position.StopLoss > li_16 || position.StopLoss == null)
                                        {
                                            if (!ModifyPosition(position, li_16, position.TakeProfit).IsSuccessful)
                                                Print("Sell 'Reversal SL/Reversal Trailing' error at: ", li_16, ", ", LastResult.Error);
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }
        //+------------------------------------------------------------------+
        //| Volume setup function                                            |
        //+------------------------------------------------------------------+
        private long VolumeSetup()
        {
            long Lq_52 = Symbol.NormalizeVolume(Symbol.QuantityToVolume(LotSize), RoundingMode.ToNearest);
            if (RiskPerTrade > 0.0 && PrimarySL > 0.0)
            {
                double minb = Nd(Symbol.Bid - PrimarySL * pnt_3);
                double sl = Math.Round(Symbol.Bid / Symbol.TickSize) - Math.Round(minb / Symbol.TickSize);
                double TickValue = Symbol.TickValue;
                double tick_lot = RiskPerTrade / (sl * TickValue);
                Lq_52 = Symbol.NormalizeVolume(tick_lot, RoundingMode.ToNearest);
            }
            Lq_52 = Math.Max(Lq_52, Symbol.VolumeMin);
            Lq_52 = Math.Min(Lq_52, Symbol.VolumeMax);
            return (Lq_52);
        }
        //+------------------------------------------------------------------+
        //| Get Trading hour                                                 |
        //+------------------------------------------------------------------+
        private bool GetTradingHour()
        {
            bool h_res = true;
            if (HourToHour)
            {
                if (StartHour != StopHour)
                {
                    if ((StopHour > StartHour && Time.Hour >= StartHour && Time.Hour < StopHour) || (StartHour > StopHour && (Time.Hour >= StartHour || Time.Hour < StopHour)))
                        h_res = true;
                    else
                        h_res = false;
                }
                else
                {
                    stop_case = "Start-hour and stop-hour must be different value\n";
                    st_45 = false;
                    h_res = false;
                }
            }
            return (h_res);
        }
        //+------------------------------------------------------------------+
        //| Normalize broker digits                                          |
        //+------------------------------------------------------------------+
        private double Nd(double par_a)
        {
            return (Math.Round(par_a, Symbol.Digits));
        }
        //+------------------------------------------------------------------+
        //| Size adjustment function                                         |
        //+------------------------------------------------------------------+
        double Sz(double ad_12, double ad_20)
        {
            return (Math.Round(ad_12 / pnt_3) - Math.Round(ad_20 / pnt_3));
        }
        //+------------------------------------------------------------------+
        //| Comments of chart                                                |
        //+------------------------------------------------------------------+
        private string ChartComment()
        {
            string ch_25 = "\nSMART KESTREL\nwww.fb.com/cls.fx\n";
            if (!safety_limit)
                ch_25 = ch_25 + "Safety-limit exceeded\n";
            if (spread > MaxSpread)
                ch_25 = ch_25 + "Max-spread exceeded\n";
            if (!st_45)
                ch_25 = ch_25 + "Order opening stopped because: " + stop_case + "\n";
            if (!c_hour)
                ch_25 = ch_25 + "Not trading hour\n";
            if (RiskPerTrade > 0.0 && PrimarySL == 0.0)
                ch_25 = ch_25 + "Risk-per-trade will not works if primary-sl is zero\n";
            return (ch_25);
        }
        //----------------------------THE END--------------------------------+
    }
}
