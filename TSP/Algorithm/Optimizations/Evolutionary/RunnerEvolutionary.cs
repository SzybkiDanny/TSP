using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TSP.Interface;

namespace TSP.Algorithm.Optimizations.IteratedLocalSearch
{
    public class RunnerEvolutionary : TspAlgorithmWithStopWatch
    {
        private readonly INonDeterministicAlgorithm _algorithmNonDeterministic;

        public RunnerEvolutionary(INonDeterministicAlgorithm algorithmNonDeterministic)
        {
            _algorithmNonDeterministic = algorithmNonDeterministic;
            Name = ((TspAlgorithmBase) _algorithmNonDeterministic).Name + " Evolutionary";
            _stopwatchRoutes = new Dictionary<int, Stopwatch>();
            RouteLengthLimit = ((TspAlgorithmBase) _algorithmNonDeterministic).RouteLengthLimit;
        }

        public int CountStartEvolutionary { get; set; }
        public int DurationLimit { get; set; }


        public override IList<KeyValuePair<int, int[]>> CalculateRoutes(IDictionary<int, int>[] distances)
        {
            Distances = distances;

            var result = new List<KeyValuePair<int, int[]>>();

            for (var i = 0; i < CountStartEvolutionary; i++)
            {
                _stopwatchRoutes[i] = new Stopwatch();
                _stopwatchRoutes[i].Start();

                var ils = new IteratedLocalSearch(_algorithmNonDeterministic)
                {
                    DurationLimit = DurationLimit
                };

                ils.CalculateRoutes(distances);
                _stopwatchRoutes[i].Stop();

                result.Add(ils.Routes.OrderBy(r => CalculateRouteLength(r.Value)).First());
            }

            IsOptimized = true;
            IsCalculated = true;
            CalculatedRoutes = result;
            return result;
        }
    }
}