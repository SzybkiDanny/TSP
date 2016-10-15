using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TSP.Algorithm.Optimizations
{
    public class LocalSearch : TspAlgorithmBase
    {
        private readonly TspAlgorithmBase _algorithm;
        private Stopwatch _stopwatch;
        public bool IsOptimized { get; private set; }

        public LocalSearch(TspAlgorithmBase algorithm)
        {
            Name = algorithm.Name + "LocalSearch";
            _algorithm = algorithm;
        }

        private void AssertIsOptimized()
        {
            if (!IsOptimized)
                throw new Exception();
        }

        public override IDictionary<int, int[]> CalculateRoutes(IDictionary<int, int>[] distances)
        {
            Distances = distances;

            var result = Optimize(_algorithm.CalculateRoutes(distances));

            IsCalculated = true;
            CalculatedRoutes = result;
            return result;
        }

        private IDictionary<int, int[]> Optimize(IDictionary<int, int[]> calculatedRoutes)
        {
            var result = new SortedList<int, int[]>();
            _stopwatch=new Stopwatch();
            _stopwatch.Start();
            for (var i = 0; i < calculatedRoutes.Count; i++)
            {
                var optimizedRoute = OptimizeRouteFromCity(i, calculatedRoutes[i]);
                result[optimizedRoute.First()] = optimizedRoute; // if it fails, then use Add method
            }
            _stopwatch.Stop();
            IsOptimized = true;
            return result;
        }

        private int[] OptimizeRouteFromCity(int startIndex, int[] route)
        {
            var minDelta = int.MaxValue;
            var oldCityIndex = 0;
            var newCity = 0;

            do
            {
                minDelta = int.MaxValue;

                for (var i = 1; i < route.Length - 1; i++)
                    for (var j = 0; j < Distances.Length; j++)
                    {
                        if (route.Contains(j))
                            continue;

                        var delta = Distances[route[i - 1]][j]
                                    + Distances[j][route[i + 1]]
                                    - Distances[route[i - 1]][route[i]]
                                    - Distances[route[i]][route[i + 1]];

                        if ((delta >= minDelta) || (delta >= 0))
                            continue;

                        minDelta = delta;
                        oldCityIndex = i;
                        newCity = j;
                    }

                if (minDelta < 0)
                    route[oldCityIndex] = newCity;
            } while (minDelta < 0);
            return route;
        }

        public string GetTimeOptimalizationAllRoutes
        {
            get
            {
                AssertIsOptimized();
                return _stopwatch.ElapsedMilliseconds + " ms";
            }
        }
    }
}