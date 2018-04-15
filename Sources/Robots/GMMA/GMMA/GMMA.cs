using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class GMMA : Robot
    {
        private ExponentialMovingAverage[] fastEma = new ExponentialMovingAverage[6];
        private ExponentialMovingAverage[] slowEma = new ExponentialMovingAverage[6];

        protected override void OnStart()
        {
            fastEma[0] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 3);
            fastEma[1] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 5);
            fastEma[2] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 8);
            fastEma[3] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 10);
            fastEma[4] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 12);
            fastEma[5] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 15);

            slowEma[0] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 30);
            slowEma[1] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 35);
            slowEma[2] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 40);
            slowEma[3] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 45);
            slowEma[4] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 50);
            slowEma[5] = Indicators.ExponentialMovingAverage(MarketSeries.Close, 60);


        }

        protected override void OnBar()
        {

            if (isLongSignalDetected() == true && isSignalConfirmed() == true)
            {
                Print("==> longsignal detected");
            }
        }

        protected override void OnStop()
        {
            // Put your deinitialization logic here
        }

        private bool isLongSignalDetected()
        {
            bool fastEmaLongOrder = true;
            for (int i = 0; i < fastEma.Length - 1; i++)
            {
                if (fastEma[i].Result.LastValue < fastEma[i + 1].Result.LastValue)
                {
                    fastEmaLongOrder = false;
                }
            }

            bool slowEmaLongOrder = true;
            for (int i = 0; i < fastEma.Length - 1; i++)
            {
                if (slowEma[i].Result.LastValue > slowEma[i + 1].Result.LastValue)
                {
                    slowEmaLongOrder = false;
                }
            }

            return (fastEmaLongOrder && slowEmaLongOrder);
        }

        private bool isSignalConfirmed()
        {
            return true;
        }
    }
}
