using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TSP.Algorithm.Optimizations
{
    public class LocalSearch : TspAlgorithmBase
    {
        private readonly TspAlgorithmBase _algorithm;
        private Dictionary<int, Stopwatch> _stopwatchRoutes;
        public bool IsOptimized { get; private set; }

        public LocalSearch(TspAlgorithmBase algorithm)
        {
            Name = algorithm.Name + "LocalSearch";
            _algorithm = algorithm;
            _stopwatchRoutes = new Dictionary<int, Stopwatch>();
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

            for (var i = 0; i < calculatedRoutes.Count; i++)
            {
                var optimizedRoute = OptimizeRouteFromCity(i, calculatedRoutes[i]);
                result[optimizedRoute.First()] = optimizedRoute;
            }

            IsOptimized = true;
            return result;
        }

        private int[] OptimizeRouteFromCity(int startIndex, int[] route)
        {
            var minDelta = int.MaxValue;
            
            _stopwatchRoutes[startIndex] = new Stopwatch();
            _stopwatchRoutes[startIndex].Start();

            do
            {
                int cityExchangeDelta = int.MaxValue,
                    oldCityIndex = 0,
                    newCity = 0;

                for (var i = 1; i < route.Length - 1; i++)
                {
                    for (var j = 0; j < Distances.Length; j++)
                    {
                        if (route.Contains(j))
                            continue;

                        var delta = Distances[route[i - 1]][j]
                                    + Distances[j][route[i + 1]]
                                    - Distances[route[i - 1]][route[i]]
                                    - Distances[route[i]][route[i + 1]];

                        if (delta >= cityExchangeDelta || delta >= 0)
                            continue;

                        cityExchangeDelta = delta;
                        oldCityIndex = i;
                        newCity = j;
                    }
                }

                int routeExchangeDelta = int.MaxValue,
                    firstRouteIndex = 0,
                    secondRouteIndex = 0;

                for (var i = 0; i < route.Length - 2; i++)
                {
                    for (var j = i + 2; j < route.Length - 1; j++)
                    {
                        var delta = Distances[route[i]][route[j]]
                                    + Distances[route[i + 1]][route[j + 1]]
                                    - Distances[route[i]][route[i + 1]]
                                    - Distances[route[j]][route[j + 1]];

                        if (delta >= routeExchangeDelta || delta >= 0)
                            continue;

                        routeExchangeDelta = delta;
                        firstRouteIndex = i;
                        secondRouteIndex = j;
                    }
                }
                if (routeExchangeDelta < 0 && routeExchangeDelta <= cityExchangeDelta)
                {
                    var routeList = route.ToList();
                    routeList.Reverse(firstRouteIndex + 1, secondRouteIndex - firstRouteIndex);
                    route = routeList.ToArray();
                }
                else if (cityExchangeDelta < 0)
                    route[oldCityIndex] = newCity;

                minDelta = Math.Min(cityExchangeDelta, routeExchangeDelta);

            } while (minDelta < 0);
            
            _stopwatchRoutes[startIndex].Stop();
            return route;
        }

        public string GetTimeOptimalizationAllRoutes
        {
            get
            {
                AssertIsOptimized();
                return _stopwatchRoutes.Sum(q => q.Value.ElapsedMilliseconds) + " ms";
            }
        }

        public string GetMinTimeOptimalization
        {
            get
            {
                AssertIsOptimized();
                return _stopwatchRoutes.Min(q => q.Value.ElapsedMilliseconds) + " ms";
            }
        }

        public string GetMaxTimeOptimalization
        {
            get
            {
                AssertIsOptimized();
                return _stopwatchRoutes.Max(q => q.Value.ElapsedMilliseconds) + " ms";
            }
        }

        public string GetAvgTimeOptimalization
        {
            get
            {
                AssertIsOptimized();
                return _stopwatchRoutes.Average(q => q.Value.ElapsedMilliseconds) + " ms";
            }
        }

        
    }
}