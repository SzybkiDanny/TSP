using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TSP.Algorithm
{
    public abstract class TspAlgorithmWithStopWatch : TspAlgorithmBase
    {
        protected IDictionary<int, Stopwatch> _stopwatchRoutes;

        public TspAlgorithmWithStopWatch()
        {
            _stopwatchRoutes = new Dictionary<int, Stopwatch>();
        }

        public bool IsOptimized { get; protected set; }

        public long GetTimeOptimalizationAllRoutes
        {
            get
            {
                AssertIsOptimized();
                return _stopwatchRoutes.Sum(q => q.Value.ElapsedMilliseconds);
            }
        }

        public long GetMinTimeOptimalization
        {
            get
            {
                AssertIsOptimized();
                return _stopwatchRoutes.Min(q => q.Value.ElapsedMilliseconds);
            }
        }

        public long GetMaxTimeOptimalization
        {
            get
            {
                AssertIsOptimized();
                return _stopwatchRoutes.Max(q => q.Value.ElapsedMilliseconds);
            }
        }

        public long GetAvgTimeOptimalization
        {
            get
            {
                AssertIsOptimized();
                return (long)_stopwatchRoutes.Average(q => q.Value.ElapsedMilliseconds);
            }
        }

        private void AssertIsOptimized()
        {
            if (!IsOptimized)
                throw new Exception();
        }
    }
}