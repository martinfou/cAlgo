using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;


//This algo opens 2 pending operations at the beginning of each days. 
//The pending price is based on the last candle min and max + 2 pips.
//WHen a pending operation gets triggered we cancel the oppostie one.
//TODO: find a way to cancel a pending order
namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class FiftyPips : Robot
    {
        [Parameter(DefaultValue = true)]
        public bool Parameter { get; set; }

        [Parameter(DefaultValue = 50)]
        public int takeProfit { get; set; }

        [Parameter(DefaultValue = 50)]
        public int stopLoss { get; set; }

        private bool allowedToTrade = false;
        private int index;
        protected override void OnStart()
        {
            // Put your initialization logic here
        }

        protected override void OnBar()
        {
            if (Server.Time.Hour == 8)
            {
                allowedToTrade = true;
                Print("London wake up and smell the cofee");
                index = MarketSeries.Close.Count - 2;
                Print("Market Serie high" + MarketSeries.High[index]);
                Print("Market Serie low" + MarketSeries.Low[index]);

                //TradeResult tr = PlaceStopOrder(TradeType.Buy, Symbol, 1000, MarketSeries.High[index], "label1", 50, 50);

            }

        }

        protected override void OnTick()
        {
            if (allowedToTrade == true)
            {
                index = MarketSeries.Close.Count - 2;
                TradeResult tr = PlaceStopOrder(TradeType.Buy, Symbol, 1000, MarketSeries.High[index], "label1", takeProfit, stopLoss);

                allowedToTrade = false;
                //TradeResult tr = PlaceStopOrder(TradeType.Sell, Symbol, 1000, MarketSeries.High[index], "label1", 50, 50);
            }
        }

        protected override void OnStop()
        {
            // Put your deinitialization logic here
        }
    }
}
