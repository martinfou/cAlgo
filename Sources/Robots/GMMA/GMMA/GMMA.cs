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
        [Parameter(DefaultValue = 0.0)]
        public double Parameter { get; set; }

        [Output("Short EMA1", Color = Colors.Blue)]
        public IndicatorDataSeries ShortEma1 { get; set; }

        [Output("Short EMA2", Color = Colors.Blue)]
        public IndicatorDataSeries ShortEma2 { get; set; }

        [Output("Short EMA3", Color = Colors.Blue)]
        public IndicatorDataSeries ShortEma3 { get; set; }

        [Output("Short EMA4", Color = Colors.Blue)]
        public IndicatorDataSeries ShortEma4 { get; set; }

        [Output("Short EMA5", Color = Colors.Blue)]
        public IndicatorDataSeries ShortEma5 { get; set; }

        [Output("Short EMA6", Color = Colors.Blue)]
        public IndicatorDataSeries ShortEma6 { get; set; }

        private ExponentialMovingAverage m_shortEma1;
        private ExponentialMovingAverage m_shortEma2;
        private ExponentialMovingAverage m_shortEma3;
        private ExponentialMovingAverage m_shortEma4;
        private ExponentialMovingAverage m_shortEma5;
        private ExponentialMovingAverage m_shortEma6;

        private ExponentialMovingAverage m_longEma1;
        private ExponentialMovingAverage m_longEma2;
        private ExponentialMovingAverage m_longEma3;
        private ExponentialMovingAverage m_longEma4;
        private ExponentialMovingAverage m_longEma5;
        private ExponentialMovingAverage m_longEma6;


        protected override void OnStart()
        {
            m_shortEma1 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 3);
            m_shortEma2 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 5);
            m_shortEma3 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 8);
            m_shortEma4 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 10);
            m_shortEma5 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 12);
            m_shortEma6 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 15);

            m_longEma1 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 30);
            m_longEma2 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 35);
            m_longEma3 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 40);
            m_longEma4 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 45);
            m_longEma5 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 50);
            m_longEma6 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 60);


        }

        protected override void OnBar()
        {
            m_shortEma1 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 3);
            m_shortEma2 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 5);
            m_shortEma3 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 8);
            m_shortEma4 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 10);
            m_shortEma5 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 12);
            m_shortEma6 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 15);

            m_longEma1 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 30);
            m_longEma2 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 35);
            m_longEma3 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 40);
            m_longEma4 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 45);
            m_longEma5 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 50);
            m_longEma6 = Indicators.ExponentialMovingAverage(MarketSeries.Close, 60);

            if (isSignalDetected() == true && isSignalConfirmed() == true)
            {
            }
        }

        protected override void OnStop()
        {
            // Put your deinitialization logic here
        }

        private bool isSignalDetected()
        {
            if (m_shortEma1.Result.HasCrossedBelow(m_longEma6.Result, 0))
            {
                Print("Short crossed below Long");
            }
            return true;
        }

        private bool isSignalConfirmed()
        {
            return true;
        }
    }
}
