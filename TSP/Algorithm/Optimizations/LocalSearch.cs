using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TSP.Algorithm.Optimizations
{
    public class LocalSearch : TspAlgorithmBase
    {
        private readonly TspAlgorithmBase _algorithm;

        public LocalSearch(TspAlgorithmBase algorithm)
        {
            _algorithm = algorithm;
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

            for (var i = 0; i < calculatedRoutes.Count; i++)
            {
                var optimizedRoute = OptimizeRouteFromCity(i, calculatedRoutes[i]);
                result[optimizedRoute.First()] = optimizedRoute; // if it fails, then use Add method
            }

            return result;
        }

        private int[] OptimizeRouteFromCity(int startIndex, int[] route)
        {
            var minDelta = int.MaxValue;
            var oldCityIndex = 0;
            var newCity = 0;

            for (var i = 1; i < route.Length; i++)
            {
                for (var j = 0; j < Distances.Length; j++)
                {
                    if (route.Contains(j))
                        continue;

                    var delta = Distances[route[i - 1]][j]
                                + Distances[j][route[i + 1]]
                                - Distances[route[i - 1]][route[i]]
                                - Distances[route[i]][route[i + 1]];

                    if (delta >= minDelta || delta >= 0)
                        continue;

                    minDelta = delta;
                    oldCityIndex = i;
                    newCity = j;
                }
            }

            if (minDelta < 0)
            {
                route[oldCityIndex] = newCity;
            }
            return null;
        }
    }
}