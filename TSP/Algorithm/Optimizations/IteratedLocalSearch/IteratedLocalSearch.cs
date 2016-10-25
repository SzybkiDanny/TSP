using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TSP.Interface;

namespace TSP.Algorithm.Optimizations.IteratedLocalSearch
{
    public class IteratedLocalSearch : TspAlgorithmWithStopWatch
    {
        private readonly Random _random = new Random();

        private LocalSearch _algorithmLocalSearch;

        public int DurationLimit { get; set; }

        private INonDeterministicAlgorithm _algorithmNonDeterministic { get; }

        public IteratedLocalSearch(INonDeterministicAlgorithm algorithmNonDeterministic)
        {
            _algorithmNonDeterministic = algorithmNonDeterministic;
            RouteLengthLimit = ((TspAlgorithmBase) _algorithmNonDeterministic).RouteLengthLimit;
            ((TspAlgorithmBase) _algorithmNonDeterministic).RouteLengthLimit = RouteLengthLimit;
            Name = ((TspAlgorithmBase) _algorithmNonDeterministic).Name + " IteratedLocalSearch";
        }

        public override IList<KeyValuePair<int, int[]>> CalculateRoutes(
            IDictionary<int, int>[] distances)
        {
            return CalculateRoutes(distances, TimeSpan.FromSeconds(DurationLimit));
        }

        public IList<KeyValuePair<int, int[]>> CalculateRoutes(IDictionary<int, int>[] distances,
            TimeSpan durationLimit)
        {
            Distances = distances;

            ((TspAlgorithmBase) _algorithmNonDeterministic).Distances = distances;
            _algorithmLocalSearch = new LocalSearch((TspAlgorithmBase) _algorithmNonDeterministic)
            {
                RouteLengthLimit = RouteLengthLimit
            };
            _algorithmLocalSearch.Distances = distances;

            var result = GenerateRoutes(durationLimit);

            IsOptimized = true;
            IsCalculated = true;
            CalculatedRoutes = result;

            return result;
        }

        private IList<KeyValuePair<int, int[]>> GenerateRoutes(TimeSpan durationLimit)
        {
            var result = new List<KeyValuePair<int, int[]>>();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _stopwatchRoutes[0] = new Stopwatch();
            _stopwatchRoutes[0].Start();

            var initialRoute = CalculateRoutesFromCity(_random.Next(Distances.Length));
            var optimizedRoute = _algorithmLocalSearch.OptimizeRouteFromCity(initialRoute.First(),
                initialRoute);

            do
            {
                var modifiedRoute = Perturbate(optimizedRoute);

                modifiedRoute = _algorithmLocalSearch.OptimizeRouteFromCity(modifiedRoute.First(),
                    modifiedRoute);

                if (CalculateRouteLength(modifiedRoute) < CalculateRouteLength(optimizedRoute))
                    optimizedRoute = modifiedRoute;
            } while (stopwatch.Elapsed <= durationLimit);

            _stopwatchRoutes[0].Stop();

            result.Add(new KeyValuePair<int, int[]>(optimizedRoute.First(), optimizedRoute));


            return result;
        }

        private int[] Perturbate(int[] route)
        {
            var routeList = route.ToList();

            var firstIndex = _random.Next(1, routeList.Count - 7);
            var secondIndex = _random.Next(firstIndex + 4, routeList.Count - 3);
            var firstCount = _random.Next(2, secondIndex - firstIndex - 1);
            var secondCount = _random.Next(2, routeList.Count - secondIndex);

            var secondRange = routeList.GetRange(secondIndex, secondCount);
            routeList.RemoveRange(secondIndex, secondCount);

            var firstRange = routeList.GetRange(firstIndex, firstCount);
            routeList.RemoveRange(firstIndex, firstCount);

            routeList.InsertRange(firstIndex, secondRange);
            routeList.InsertRange(secondIndex + secondCount - firstCount, firstRange);

            return routeList.ToArray();
        }

        public int[] CalculateRoutesFromCity(int cityIndex)
        {
            return _algorithmNonDeterministic.CalculateRoutesFromCity(cityIndex);
        }
    }
}