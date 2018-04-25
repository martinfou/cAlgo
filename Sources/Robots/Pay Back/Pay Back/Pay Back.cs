using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;
//https://ctdn.com/algos/cbots/show/192
namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PayBacK : Robot
    {

        ///////////////////////////////////////////////////////

        [Parameter("SETTING BUY", DefaultValue = "___BUY___")]
        public string Separator1 { get; set; }

        [Parameter("Robot Label", DefaultValue = "buy eurusd")]
        public string RobotID1 { get; set; }

        //////////////////////////////////////////////////////

        [Parameter("Start Buy", DefaultValue = true)]
        public bool Buy { get; set; }

        [Parameter("Quantity initial Buy (Lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double Quantity1 { get; set; }

        [Parameter("Stop Loss", DefaultValue = 80)]
        public double StopLoss { get; set; }

        [Parameter("Take Profit", DefaultValue = 80)]
        public double TakeProfit { get; set; }

        [Parameter("Start Martingale Buy", DefaultValue = true)]
        public bool StartMartingaleBuy { get; set; }

        [Parameter("Change the direction Martingale", DefaultValue = false)]
        public bool change1 { get; set; }

        [Parameter("Multiplier", DefaultValue = 2.1)]
        public double Multiplier { get; set; }

        [Parameter("Max Volume Buy.....Return ", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity1Max { get; set; }

        [Parameter("Start Automate Buy", DefaultValue = true)]
        public bool StartAutomate1 { get; set; }


        ///////////////////////////////////////////////////////

        [Parameter("SETTING SELL", DefaultValue = "___SELL___")]
        public string Separator2 { get; set; }

        [Parameter("Robot Label", DefaultValue = "sell eurusd")]
        public string RobotID2 { get; set; }

        //////////////////////////////////////////////////////

        [Parameter("Start Sell", DefaultValue = true)]
        public bool Sell { get; set; }

        [Parameter("Quantity initial Sell (Lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double Quantity2 { get; set; }

        [Parameter("Stop Loss", DefaultValue = 80)]
        public double StopLoss2 { get; set; }

        [Parameter("Take Profit", DefaultValue = 80)]
        public double TakeProfit2 { get; set; }

        [Parameter("Start Martingale Sell", DefaultValue = true)]
        public bool StartMartingaleSell { get; set; }

        [Parameter("change the direction Martingale", DefaultValue = false)]
        public bool change2 { get; set; }

        [Parameter("Multiplier", DefaultValue = 2.1)]
        public double Multiplier2 { get; set; }

        [Parameter("Max Volume Sell.....Return", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity2Max { get; set; }

        [Parameter("Start Automate Sell", DefaultValue = true)]
        public bool StartAutomate2 { get; set; }



        ///////////////////////////////////////////////////////

        [Parameter("SETTING STOP ORDER", DefaultValue = "___STOP ORDER___")]
        public string Separator3 { get; set; }

        [Parameter("Robot Label", DefaultValue = "stop eurusd")]
        public string RobotID3 { get; set; }

        //////////////////////////////////////////////////////////

        [Parameter("Start StopOrder", DefaultValue = true)]
        public bool stoporder { get; set; }

        [Parameter("Quantity initial StopOrder (Lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double Quantity3 { get; set; }

        [Parameter("PipsAway", DefaultValue = 10)]
        public double PipsAway { get; set; }

        [Parameter("Stop Loss StopOrder ", DefaultValue = 60)]
        public double StopLoss3 { get; set; }

        [Parameter("Take Profit StopOrder", DefaultValue = 60)]
        public double TakeProfit3 { get; set; }

        [Parameter("Start Martingale StopOrder", DefaultValue = true)]
        public bool StartMartingaleStopOrder { get; set; }

        [Parameter("Martingale NONSTOP", DefaultValue = false)]
        public bool MartingaleNonStop1 { get; set; }

        [Parameter("change the direction Martingale", DefaultValue = false)]
        public bool change3 { get; set; }

        [Parameter("Multiplier", DefaultValue = 2.1)]
        public double Multiplier3 { get; set; }

        [Parameter("Max Volume StopOrder", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity3Max { get; set; }

        [Parameter("Start Automate StopOrder", DefaultValue = true)]
        public bool StartAutomate3 { get; set; }


        ///////////////////////////////////////////////////////


        [Parameter("SETTING LIMIT ORDER", DefaultValue = "___LIMIT ORDER___")]
        public string Separator4 { get; set; }

        [Parameter("Robot Label", DefaultValue = "limit eurusd")]
        public string RobotID4 { get; set; }

        //////////////////////////////////////////////////////////

        [Parameter("Start LimitOrder", DefaultValue = true)]
        public bool pendingorder { get; set; }

        [Parameter("Quantity initial LimitOrder (Lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double Quantity4 { get; set; }

        [Parameter("PipsAway", DefaultValue = 20)]
        public double PipsAway2 { get; set; }

        [Parameter("Stop Loss LimitOrder ", DefaultValue = 60)]
        public double StopLoss4 { get; set; }

        [Parameter("Take Profit LimitOrder", DefaultValue = 60)]
        public double TakeProfit4 { get; set; }

        [Parameter("Start Martingale LimitOrder", DefaultValue = true)]
        public bool StartMartingalePendingOrder { get; set; }

        [Parameter("Martingale NONSTOP", DefaultValue = false)]
        public bool MartingaleNonStop2 { get; set; }

        [Parameter("change the direction Martingale", DefaultValue = false)]
        public bool change4 { get; set; }

        [Parameter("Multiplier", DefaultValue = 2.1)]
        public double Multiplier4 { get; set; }

        [Parameter("Max Volume LimitOrder", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity4Max { get; set; }

        [Parameter("Start Automate LimitOrder", DefaultValue = true)]
        public bool StartAutomate4 { get; set; }

        ///////////////////////////////////////////////////////////////////////////

        public long volumeMax1;
        public long volumeMax2;
        public long volumeMax3;
        public long volumeMax4;

        public double earn1;
        public double earn2;
        public double earn3;
        public double earn4;

        bool Oco = true;
        bool Oco2 = true;


        protected override void OnStart()
        {

            string text = "PayBacK By TraderMatriX";

            base.ChartObjects.DrawText("PayBacK By TraderMatriX", text, StaticPosition.TopCenter, new Colors?(Colors.Lime));


            buy();
            Positions.Closed += OnPositionsClosed1;
            Positions.Closed += OnPositionsClosed2;
            Positions.Closed += OnPositionsClosedReturnBuy;


            sell();
            Positions.Closed += OnPositionsClosed3;
            Positions.Closed += OnPositionsClosed4;
            Positions.Closed += OnPositionsClosedReturnSell;

            StopOrders();
            Positions.Closed += OnPositionsClosed5;
            Positions.Closed += OnPositionsClosed6;
            Positions.Closed += OnPositionsClosedReturnStop;
            Positions.Closed += OnPositionsClosedNonStop1A;
            Positions.Closed += OnPositionsClosedNonStop1B;
            Positions.Opened += OnPositionOpened;

            LimitOrder();
            Positions.Closed += OnPositionsClosed7;
            Positions.Closed += OnPositionsClosed8;
            Positions.Closed += OnPositionsClosedReturnLimit;
            Positions.Closed += OnPositionsClosedNonStop2A;
            Positions.Closed += OnPositionsClosedNonStop2B;
            Positions.Opened += OnPositionOpened2;

            var volumeInUnits1Max = Symbol.QuantityToVolume(Quantity1Max);

            volumeMax1 = volumeInUnits1Max;

            var volumeInUnits2Max = Symbol.QuantityToVolume(Quantity2Max);

            volumeMax2 = volumeInUnits2Max;

            var volumeInUnits3Max = Symbol.QuantityToVolume(Quantity3Max);

            volumeMax3 = volumeInUnits3Max;

            var volumeInUnits4Max = Symbol.QuantityToVolume(Quantity4Max);

            volumeMax4 = volumeInUnits4Max;

            DisplayEarn();

        }


        protected override void OnTick()
        {


            var netProfit = 0.0;


            foreach (var openedPosition in Positions)
            {

                netProfit += openedPosition.NetProfit + openedPosition.Commissions;


            }

            ChartObjects.DrawText("a", netProfit.ToString(), StaticPosition.BottomRight, new Colors?(Colors.Lime));

            DisplayEarn();

            NetProfit1();
            NetProfit2();
            NetProfit3();
            NetProfit4();



        }

        private string GenerateText()
        {

            var e1 = "";
            var e2 = "";
            var e3 = "";
            var e4 = "";

            var earnText = "";

            e1 = "\nBuy = " + earn1;
            e2 = "\nSell = " + earn2;
            e3 = "\nStopOrder = " + earn3;
            e4 = "\nLimitOrder= " + earn4;


            earnText = e1 + e2 + e3 + e4;
            return (earnText);
        }

        private void DisplayEarn()
        {

            ChartObjects.DrawText("text", GenerateText(), StaticPosition.TopRight, Colors.Aqua);
        }

        private void NetProfit1()
        {

            earn1 = 0;



            foreach (var pos in Positions)
            {

                if (pos.Label.StartsWith(RobotID1))
                {
                    earn1 += pos.NetProfit + pos.Commissions;
                }

            }
        }



        private void NetProfit2()
        {


            earn2 = 0;

            foreach (var pos2 in Positions)
            {
                if (pos2.Label.StartsWith(RobotID2))
                {
                    earn2 += pos2.NetProfit + pos2.Commissions;
                }
            }
        }

        private void NetProfit3()
        {

            earn3 = 0;



            foreach (var pos3 in Positions)
            {
                if (pos3.Label.StartsWith(RobotID3))
                {
                    earn3 += pos3.NetProfit + pos3.Commissions;
                }
            }
        }

        private void NetProfit4()
        {


            earn4 = 0;



            foreach (var pos4 in Positions)
            {
                if (pos4.Label.StartsWith(RobotID4))
                {
                    earn4 += pos4.NetProfit + pos4.Commissions;
                }
            }
        }

        private void buy()
        {


            if (Buy == true)
            {

                var cBotPositions = Positions.FindAll(RobotID1);

                if (cBotPositions.Length >= 1)
                    return;


                var volumeInUnits1 = Symbol.QuantityToVolume(Quantity1);


                ExecuteMarketOrder(TradeType.Buy, Symbol, volumeInUnits1, RobotID1, StopLoss, TakeProfit);
                Print("ExecuteMarketOrder,Quantity initial Buy");
            }
        }

        private void sell()
        {
            if (Sell == true)
            {

                var cBotPositions = Positions.FindAll(RobotID2);

                if (cBotPositions.Length >= 1)
                    return;


                var volumeInUnits2 = Symbol.QuantityToVolume(Quantity2);

                ExecuteMarketOrder(TradeType.Sell, Symbol, volumeInUnits2, RobotID2, StopLoss2, TakeProfit2);
                Print("ExecuteMarketOrder,Quantity initial Sell");

            }
        }

        private void StopOrders()
        {

            if (stoporder == true)
            {
                var volumeInUnits3 = Symbol.QuantityToVolume(Quantity3);


                var sellOrderTargetPrice = Symbol.Bid - PipsAway * Symbol.PipSize;

                ChartObjects.DrawHorizontalLine("sell target", sellOrderTargetPrice, Colors.Lime, 1, LineStyle.Solid);

                PlaceStopOrder(TradeType.Sell, Symbol, volumeInUnits3, sellOrderTargetPrice, RobotID3, StopLoss3, TakeProfit3);
                Print("PlaceStopOrder Sell,Quantity initial Stop Order");


                var buyOrderTargetPrice = Symbol.Ask + PipsAway * Symbol.PipSize;

                ChartObjects.DrawHorizontalLine("buy target", buyOrderTargetPrice, Colors.Lime, 1, LineStyle.Solid);

                PlaceStopOrder(TradeType.Buy, Symbol, volumeInUnits3, buyOrderTargetPrice, RobotID3, StopLoss3, TakeProfit3);
                Print("PlaceStopOrder Buy,Quantity initial Stop Order");

            }

        }


        private void LimitOrder()
        {

            if (pendingorder == true)
            {



                var volumeInUnits4 = Symbol.QuantityToVolume(Quantity4);

                var sellOrderTargetPrice = Symbol.Bid + PipsAway2 * Symbol.PipSize;

                ChartObjects.DrawHorizontalLine("sell target2", sellOrderTargetPrice, Colors.Silver, 1, LineStyle.Solid);

                PlaceLimitOrder(TradeType.Sell, Symbol, volumeInUnits4, sellOrderTargetPrice, RobotID4, StopLoss4, TakeProfit4);
                Print("PlaceLimitOrder Sell,Quantity initial Limit Order");



                var buyOrderTargetPrice = Symbol.Ask - PipsAway2 * Symbol.PipSize;

                ChartObjects.DrawHorizontalLine("buy target2", buyOrderTargetPrice, Colors.Silver, 1, LineStyle.Solid);

                PlaceLimitOrder(TradeType.Buy, Symbol, volumeInUnits4, buyOrderTargetPrice, RobotID4, StopLoss4, TakeProfit4);
                Print("PlaceLimitOrder Buy,Quantity initial Limit Order");


            }

        }

        private void OnPositionsClosed1(PositionClosedEventArgs args)
        {
            if (Buy == true)

                if (StartMartingaleBuy == true)

                    if (StartAutomate1 == true)
                    {

                        Print("martingale active + Automate Active...buy PayBack");

                        var position = args.Position;

                        if (position.Label != RobotID1 || position.SymbolCode != Symbol.Code)
                            return;


                        if (position.Pips > 0)
                            buy();
                        {




                            if (position.GrossProfit < 0)
                            {


                                if (change1 == true)
                                {

                                    TradeType AA = TradeType.Sell;

                                    if (position.TradeType == TradeType.Sell)

                                        AA = TradeType.Buy;



                                    if (position.Volume * Multiplier <= volumeMax1)

                                        ExecuteMarketOrder(AA, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier), RobotID1, StopLoss, TakeProfit);
                                    Print("loss;inverse direction buy PayBack");
                                }

                                else if (change1 == false)
                                {
                                    TradeType BB = TradeType.Sell;



                                    BB = TradeType.Buy;


                                    if (position.Volume * Multiplier <= volumeMax1)

                                        ExecuteMarketOrder(BB, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier), RobotID1, StopLoss, TakeProfit);
                                    Print("loss; NO inverse direction buy PayBack");

                                }

                            }

                        }

                    }
        }




        private void OnPositionsClosed2(PositionClosedEventArgs args)
        {

            if (Buy == true)

                if (StartMartingaleBuy == true)

                    if (StartAutomate1 == false)
                    {

                        Print("martingale active + Automate inactive...buy PayBack");

                        var position = args.Position;

                        if (position.Label != RobotID1 || position.SymbolCode != Symbol.Code)
                            return;


                        if (position.Pips > 0)
                            return;
                        {

                            if (position.GrossProfit < 0)
                            {

                                if (change1 == true)
                                {
                                    TradeType AA = TradeType.Sell;

                                    if (position.TradeType == TradeType.Sell)

                                        AA = TradeType.Buy;

                                    if (position.Volume * Multiplier <= volumeMax1)


                                        ExecuteMarketOrder(AA, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier), RobotID1, StopLoss, TakeProfit);
                                    Print("loss; inverse direction buy PayBack");
                                }


                                else if (change1 == false)
                                {
                                    TradeType BB = TradeType.Sell;


                                    BB = TradeType.Buy;

                                    if (position.Volume * Multiplier <= volumeMax1)


                                        ExecuteMarketOrder(BB, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier), RobotID1, StopLoss, TakeProfit);
                                    Print("loss; NO inverse direction buy PayBack");



                                }

                            }

                        }

                    }

        }


        private void OnPositionsClosedReturnBuy(PositionClosedEventArgs args)
        {
            if (Buy == true)
                if (StartMartingaleBuy == true)

                    if (StartAutomate1 == true)
                    {

                        var volumeInUnits1Max = Symbol.QuantityToVolume(Quantity1Max);


                        var position = args.Position;

                        if (position.Label != RobotID1 || position.SymbolCode != Symbol.Code)
                            return;
                        if (position.Volume * Multiplier >= volumeInUnits1Max)

                            buy();

                    }
        }


        private void OnPositionsClosed3(PositionClosedEventArgs args)
        {

            if (Sell == true)

                if (StartMartingaleSell == true)

                    if (StartAutomate2 == true)
                    {


                        Print("martingale active + Automate Active...sell PayBack");

                        var position = args.Position;

                        if (position.Label != RobotID2 || position.SymbolCode != Symbol.Code)
                            return;


                        if (position.Pips > 0)
                            sell();
                        {


                            if (position.GrossProfit < 0)
                            {

                                if (change2 == true)
                                {
                                    TradeType AA = TradeType.Sell;

                                    if (position.TradeType == TradeType.Sell)

                                        AA = TradeType.Buy;

                                    if (position.Volume * Multiplier2 <= volumeMax2)


                                        ExecuteMarketOrder(AA, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier2), RobotID2, StopLoss2, TakeProfit2);
                                    Print("loss; inverse direction sell PayBack");
                                }

                                else if (change2 == false)
                                {
                                    TradeType BB = TradeType.Buy;



                                    BB = TradeType.Sell;

                                    if (position.Volume * Multiplier2 <= volumeMax2)


                                        ExecuteMarketOrder(BB, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier2), RobotID2, StopLoss2, TakeProfit2);
                                    Print("loss; NO inverse direction sell PayBack");



                                }

                            }

                        }

                    }

        }


        private void OnPositionsClosed4(PositionClosedEventArgs args)
        {

            if (Sell == true)

                if (StartMartingaleSell == true)

                    if (StartAutomate2 == false)
                    {


                        Print("martingale active + Automate inactive...sell PayBack");

                        var position = args.Position;

                        if (position.Label != RobotID2 || position.SymbolCode != Symbol.Code)
                            return;


                        if (position.Pips > 0)
                            return;
                        {

                            if (position.GrossProfit < 0)
                            {

                                if (change2 == true)
                                {
                                    TradeType AA = TradeType.Sell;

                                    if (position.TradeType == TradeType.Sell)

                                        AA = TradeType.Buy;

                                    if (position.Volume * Multiplier2 <= volumeMax2)


                                        ExecuteMarketOrder(AA, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier2), RobotID2, StopLoss2, TakeProfit2);
                                    Print("loss; inverse direction sell PayBack");
                                }


                                else if (change2 == false)
                                {
                                    TradeType BB = TradeType.Buy;



                                    BB = TradeType.Sell;


                                    if (position.Volume * Multiplier2 <= volumeMax2)

                                        ExecuteMarketOrder(BB, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier2), RobotID2, StopLoss2, TakeProfit2);
                                    Print("loss; NO inverse direction sell PayBack");



                                }

                            }

                        }

                    }

        }

        private void OnPositionsClosedReturnSell(PositionClosedEventArgs args)
        {
            if (Sell == true)
                if (StartMartingaleSell == true)

                    if (StartAutomate2 == true)
                    {

                        var volumeInUnits2Max = Symbol.QuantityToVolume(Quantity2Max);


                        var position = args.Position;

                        if (position.Label != RobotID2 || position.SymbolCode != Symbol.Code)
                            return;
                        if (position.Volume * Multiplier2 >= volumeInUnits2Max)

                            if (position.GrossProfit < 0)

                                sell();

                    }
        }



        private void OnPositionsClosed5(PositionClosedEventArgs args)
        {

            if (stoporder == true)

                if (StartMartingaleStopOrder == true)

                    if (MartingaleNonStop1 == true)

                        if (StartAutomate3 == true)
                        {


                            Print("martingale active NonStop = TRUE + Automate Active...StopOrder PayBack");

                            var position = args.Position;

                            if (position.Label != RobotID3 || position.SymbolCode != Symbol.Code)
                                return;


                            if (position.Pips > 0)
                                StopOrders();
                            {


                                if (position.GrossProfit < 0)
                                {

                                    if (change3 == true)
                                    {
                                        var cBotPositions = Positions.FindAll(RobotID3);

                                        if (cBotPositions.Length >= 1)
                                            return;
                                        if (position.Volume * Multiplier3 <= volumeMax3)


                                            ExecuteMarketOrder(position.TradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier3), RobotID3, StopLoss3, TakeProfit3);
                                        Print("loss; inverse direction StopOrder PayBack");
                                    }

                                    else if (change3 == false)
                                    {
                                        var cBotPositions = Positions.FindAll(RobotID3);

                                        if (cBotPositions.Length >= 1)
                                            return;
                                        if (position.Volume * Multiplier3 <= volumeMax3)


                                            ExecuteMarketOrder(position.TradeType == TradeType.Buy ? TradeType.Buy : TradeType.Sell, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier3), RobotID3, StopLoss3, TakeProfit3);
                                        Print("loss; NO inverse direction StopOrder PayBack");

                                    }

                                }

                            }

                        }

        }




        private void OnPositionsClosed6(PositionClosedEventArgs args)
        {

            if (stoporder == true)

                if (StartMartingaleStopOrder == true)

                    if (MartingaleNonStop1 == true)

                        if (StartAutomate3 == false)
                        {


                            Print("martingale active NonStop = TRUE + Automate inactive...StopOrder PayBack");


                            var position = args.Position;

                            if (position.Label != RobotID3 || position.SymbolCode != Symbol.Code)
                                return;


                            if (position.Pips > 0)
                                return;
                            {


                                if (position.GrossProfit < 0)
                                {

                                    if (change3 == true)
                                    {

                                        if (position.Volume * Multiplier3 <= volumeMax3)


                                            ExecuteMarketOrder(position.TradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier3), RobotID3, StopLoss3, TakeProfit3);
                                        Print("loss; inverse direction StopOrder PayBack");
                                    }

                                    else if (change3 == false)
                                    {

                                        if (position.Volume * Multiplier3 <= volumeMax3)

                                            ExecuteMarketOrder(position.TradeType == TradeType.Buy ? TradeType.Buy : TradeType.Sell, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier3), RobotID3, StopLoss3, TakeProfit3);
                                        Print("loss; NO inverse direction StopOrder PayBack");

                                    }

                                }

                            }

                        }

        }


        private void OnPositionsClosedReturnStop(PositionClosedEventArgs args)
        {
            if (stoporder == true)
                if (StartMartingaleStopOrder == true)
                    if (StartAutomate3 == true)

                        if (MartingaleNonStop1 == true)
                        {

                            var volumeInUnits3Max = Symbol.QuantityToVolume(Quantity3Max);


                            var position = args.Position;

                            if (position.Label != RobotID3 || position.SymbolCode != Symbol.Code)
                                return;


                            if (position.Volume * Multiplier3 >= volumeInUnits3Max)
                                if (position.GrossProfit < 0)
                                    StopOrders();


                        }
        }




        private void OnPositionsClosedNonStop1A(PositionClosedEventArgs args)
        {

            if (stoporder == true)

                if (StartMartingaleStopOrder == true)

                    if (MartingaleNonStop1 == false)

                        if (StartAutomate3 == true)
                        {


                            Print("martingale active NonStop = FALSE + Automate active...StopOrder PayBack");


                            var position = args.Position;

                            if (position.Label != RobotID3 || position.SymbolCode != Symbol.Code)
                                return;



                            if (position.Pips > 0)

                                if (position.Volume * Multiplier3 <= volumeMax3)
                                    StopOrders();

                            if (position.GrossProfit < 0)
                                if (position.Volume * Multiplier3 > volumeMax3)

                                    StopOrders();

                            if (position.GrossProfit > 0)
                                if (position.Volume * Multiplier3 > volumeMax3)

                                    StopOrders();


                            {


                                if (position.GrossProfit < 0)
                                {




                                    if (position.Volume * Multiplier3 <= volumeMax3)
                                    {
                                        var volumeInUnits3 = Symbol.QuantityToVolume(Quantity3);


                                        var sellOrderTargetPrice = Symbol.Bid - PipsAway * Symbol.PipSize;

                                        ChartObjects.DrawHorizontalLine("sell target", sellOrderTargetPrice, Colors.Lime, 1, LineStyle.Solid);

                                        PlaceStopOrder(TradeType.Sell, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier3), sellOrderTargetPrice, RobotID3, StopLoss3, TakeProfit3);
                                        Print("PlaceStopOrder Sell,Quantity initial Stop Order");


                                        var buyOrderTargetPrice = Symbol.Ask + PipsAway * Symbol.PipSize;

                                        ChartObjects.DrawHorizontalLine("buy target", buyOrderTargetPrice, Colors.Lime, 1, LineStyle.Solid);

                                        PlaceStopOrder(TradeType.Buy, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier3), buyOrderTargetPrice, RobotID3, StopLoss3, TakeProfit3);
                                        Print("PlaceStopOrder Buy,Quantity initial Stop Order");


                                    }
                                }
                            }
                        }
        }


        private void OnPositionsClosedNonStop1B(PositionClosedEventArgs args)
        {

            if (stoporder == true)

                if (StartMartingaleStopOrder == true)

                    if (MartingaleNonStop1 == false)

                        if (StartAutomate3 == false)
                        {


                            Print("martingale active NonStop = FALSE + Automate inactive...StopOrder PayBack");


                            var position = args.Position;

                            if (position.Label != RobotID3 || position.SymbolCode != Symbol.Code)
                                return;



                            if (position.Pips > 0)

                                if (position.Volume * Multiplier3 <= volumeMax3)
                                    return;
                            {


                                if (position.GrossProfit < 0)
                                {

                                    if (position.Volume * Multiplier3 <= volumeMax3)
                                    {
                                        var volumeInUnits3 = Symbol.QuantityToVolume(Quantity3);


                                        var sellOrderTargetPrice = Symbol.Bid - PipsAway * Symbol.PipSize;

                                        ChartObjects.DrawHorizontalLine("sell target", sellOrderTargetPrice, Colors.Lime, 1, LineStyle.Solid);

                                        PlaceStopOrder(TradeType.Sell, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier3), sellOrderTargetPrice, RobotID3, StopLoss3, TakeProfit3);
                                        Print("PlaceStopOrder Sell,Quantity initial Stop Order");


                                        var buyOrderTargetPrice = Symbol.Ask + PipsAway * Symbol.PipSize;

                                        ChartObjects.DrawHorizontalLine("buy target", buyOrderTargetPrice, Colors.Lime, 1, LineStyle.Solid);

                                        PlaceStopOrder(TradeType.Buy, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier3), buyOrderTargetPrice, RobotID3, StopLoss3, TakeProfit3);
                                        Print("PlaceStopOrder Buy,Quantity initial Stop Order");

                                    }
                                }
                            }
                        }
        }

        private void OnPositionOpened(PositionOpenedEventArgs args)
        {

            if (Oco == true)
            {
                var position = args.Position;


                if (position.Label == RobotID3 && position.SymbolCode == Symbol.Code)
                {




                    foreach (var order in PendingOrders)
                    {

                        if (order.Label == RobotID3 && order.SymbolCode == Symbol.Code)
                        {
                            CancelPendingOrderAsync(order);

                            ChartObjects.RemoveObject("sell target");
                            ChartObjects.RemoveObject("buy target");
                            Print("CancelStopOrder PayBack");

                        }

                    }

                }

            }

        }


        private void OnPositionsClosed7(PositionClosedEventArgs args)
        {

            if (pendingorder == true)

                if (StartMartingalePendingOrder == true)

                    if (MartingaleNonStop2 == true)

                        if (StartAutomate4 == true)
                        {


                            Print("martingale active NonStop = TRUE + Automate Active...LimitOrder PayBack");


                            var position = args.Position;

                            if (position.Label != RobotID4 || position.SymbolCode != Symbol.Code)
                                return;


                            if (position.Pips > 0)
                                LimitOrder();
                            {


                                if (position.GrossProfit < 0)
                                {

                                    if (change4 == true)
                                    {

                                        if (position.Volume * Multiplier4 <= volumeMax4)


                                            ExecuteMarketOrder(position.TradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier4), RobotID4, StopLoss4, TakeProfit4);
                                        Print("loss; inverse direction limitOrder PayBack");
                                    }

                                    else if (change4 == false)
                                    {

                                        if (position.Volume * Multiplier4 <= volumeMax4)


                                            ExecuteMarketOrder(position.TradeType == TradeType.Buy ? TradeType.Buy : TradeType.Sell, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier4), RobotID4, StopLoss4, TakeProfit4);
                                        Print("loss; NO inverse direction limitOrder PayBack");

                                    }

                                }

                            }

                        }

        }




        private void OnPositionsClosed8(PositionClosedEventArgs args)
        {

            if (pendingorder == true)

                if (StartMartingalePendingOrder == true)

                    if (MartingaleNonStop2 == true)


                        if (StartAutomate4 == false)
                        {


                            Print("martingale active NonStop = FALSE + Automate inactive...LimitOrder PayBack");

                            var position = args.Position;

                            if (position.Label != RobotID4 || position.SymbolCode != Symbol.Code)
                                return;


                            if (position.Pips > 0)
                                return;
                            {


                                if (position.GrossProfit < 0)
                                {

                                    if (change4 == true)
                                    {

                                        if (position.Volume * Multiplier4 <= volumeMax4)


                                            ExecuteMarketOrder(position.TradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier4), RobotID4, StopLoss4, TakeProfit4);
                                        Print("loss; inverse direction limitOrder PayBack");
                                    }

                                    else if (change4 == false)
                                    {

                                        if (position.Volume * Multiplier4 <= volumeMax4)


                                            ExecuteMarketOrder(position.TradeType == TradeType.Buy ? TradeType.Buy : TradeType.Sell, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier4), RobotID4, StopLoss4, TakeProfit4);
                                        Print("loss; NO inverse direction limitOrder PayBack");


                                    }

                                }

                            }

                        }

        }

        private void OnPositionsClosedReturnLimit(PositionClosedEventArgs args)
        {
            if (pendingorder == true)

                if (StartMartingalePendingOrder == true)

                    if (StartAutomate4 == true)

                        if (MartingaleNonStop2 == true)
                        {

                            var volumeInUnits4Max = Symbol.QuantityToVolume(Quantity4Max);


                            var position = args.Position;

                            if (position.Label != RobotID4 || position.SymbolCode != Symbol.Code)
                                return;


                            if (position.Volume * Multiplier4 >= volumeInUnits4Max)
                                if (position.GrossProfit < 0)
                                    LimitOrder();

                        }
        }

        private void OnPositionsClosedNonStop2A(PositionClosedEventArgs args)
        {

            if (pendingorder == true)

                if (StartMartingalePendingOrder == true)

                    if (MartingaleNonStop2 == false)

                        if (StartAutomate4 == true)
                        {


                            Print("martingale active NonStop = FALSE + Automate active...LimitOrder PayBack");


                            var position = args.Position;

                            if (position.Label != RobotID4 || position.SymbolCode != Symbol.Code)
                                return;



                            if (position.Pips > 0)

                                if (position.Volume * Multiplier4 <= volumeMax4)
                                    LimitOrder();

                            if (position.GrossProfit > 0)
                                if (position.Volume * Multiplier4 > volumeMax4)

                                    LimitOrder();

                            if (position.GrossProfit < 0)
                                if (position.Volume * Multiplier4 > volumeMax4)

                                    LimitOrder();

                            {


                                if (position.GrossProfit < 0)
                                {




                                    if (position.Volume * Multiplier4 <= volumeMax4)
                                    {

                                        var volumeInUnits4 = Symbol.QuantityToVolume(Quantity4);

                                        var sellOrderTargetPrice = Symbol.Bid + PipsAway2 * Symbol.PipSize;

                                        ChartObjects.DrawHorizontalLine("sell target2", sellOrderTargetPrice, Colors.Silver, 1, LineStyle.Solid);

                                        PlaceLimitOrder(TradeType.Sell, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier4), sellOrderTargetPrice, RobotID4, StopLoss4, TakeProfit4);
                                        Print("PlaceLimitOrder Sell,position.Volume * Multiplier");



                                        var buyOrderTargetPrice = Symbol.Ask - PipsAway2 * Symbol.PipSize;

                                        ChartObjects.DrawHorizontalLine("buy target2", buyOrderTargetPrice, Colors.Silver, 1, LineStyle.Solid);

                                        PlaceLimitOrder(TradeType.Buy, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier4), buyOrderTargetPrice, RobotID4, StopLoss4, TakeProfit4);
                                        Print("PlaceLimitOrder Buy,position.Volume * Multiplier");


                                    }
                                }
                            }
                        }
        }


        private void OnPositionsClosedNonStop2B(PositionClosedEventArgs args)
        {

            if (pendingorder == true)

                if (StartMartingalePendingOrder == true)

                    if (MartingaleNonStop2 == false)

                        if (StartAutomate4 == false)
                        {


                            Print("martingale active NonStop = FALSE + Automate inactive...LimitOrder PayBack");


                            var position = args.Position;

                            if (position.Label != RobotID4 || position.SymbolCode != Symbol.Code)
                                return;



                            if (position.Pips > 0)

                                if (position.Volume * Multiplier4 <= volumeMax4)
                                    return;


                            {


                                if (position.GrossProfit < 0)
                                {




                                    if (position.Volume * Multiplier4 <= volumeMax4)
                                    {

                                        var volumeInUnits4 = Symbol.QuantityToVolume(Quantity4);

                                        var sellOrderTargetPrice = Symbol.Bid + PipsAway2 * Symbol.PipSize;

                                        ChartObjects.DrawHorizontalLine("sell target2", sellOrderTargetPrice, Colors.Silver, 1, LineStyle.Solid);

                                        PlaceLimitOrder(TradeType.Sell, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier4), sellOrderTargetPrice, RobotID4, StopLoss4, TakeProfit4);
                                        Print("PlaceLimitOrder Sell,position.Volume * Multiplier");



                                        var buyOrderTargetPrice = Symbol.Ask - PipsAway2 * Symbol.PipSize;

                                        ChartObjects.DrawHorizontalLine("buy target2", buyOrderTargetPrice, Colors.Silver, 1, LineStyle.Solid);

                                        PlaceLimitOrder(TradeType.Buy, Symbol, Symbol.NormalizeVolume(position.Volume * Multiplier4), buyOrderTargetPrice, RobotID4, StopLoss4, TakeProfit4);
                                        Print("PlaceLimitOrder Buy,position.Volume * Multiplier");



                                    }
                                }
                            }
                        }
        }



        private void OnPositionOpened2(PositionOpenedEventArgs args)
        {

            if (Oco2 == true)
            {
                var position = args.Position;


                if (position.Label == RobotID4 && position.SymbolCode == Symbol.Code)
                {


                    foreach (var order in PendingOrders)
                    {

                        if (order.Label == RobotID4 && order.SymbolCode == Symbol.Code)
                        {
                            CancelPendingOrderAsync(order);

                            ChartObjects.RemoveObject("sell target2");
                            ChartObjects.RemoveObject("buy target2");
                            Print("CancelLimitOrder PayBack");

                        }

                    }

                }

            }

        }

    }

}
