using System;
using System.Collections.Generic;
using System.Linq;
using TSP.Interface;

namespace TSP.Algorithm.Optimizations.MultipleStartLocalSearch
{
    public class MultipleStartLocalSearch : TspAlgorithmBase
    {
        private readonly Random _random = new Random();

        private readonly LocalSearch _algorithmLocalSearch;
        public int? CountRepeatStartAlgorithm { get; set; }
        private INonDeterministicAlgorithm _algorithmNonDeterministic { get; }

        public MultipleStartLocalSearch(INonDeterministicAlgorithm algorithmNonDeterministic)
        {
            _algorithmNonDeterministic = algorithmNonDeterministic;
            RouteLengthLimit = ((TspAlgorithmBase) _algorithmNonDeterministic).RouteLengthLimit;
            ((TspAlgorithmBase) _algorithmNonDeterministic).RouteLengthLimit = RouteLengthLimit;
            _algorithmLocalSearch = new LocalSearch((TspAlgorithmBase) _algorithmNonDeterministic)
            {
                RouteLengthLimit = RouteLengthLimit
            };
        }

        public override IList<KeyValuePair<int, int[]>> CalculateRoutes(
            IDictionary<int, int>[] distances)
        {
            Distances = distances;
            ((TspAlgorithmBase) _algorithmNonDeterministic).Distances = distances;
            _algorithmLocalSearch.Distances = distances;

            var result = new List<KeyValuePair<int, int[]>>();


            for (var i = 0; i < CountRepeatStartAlgorithm; i++)
            {
                var routeStart = CalculateRoutesFromCity(_random.Next(Distances.Length));
                var optimizedRoute = _algorithmLocalSearch.OptimizeRouteFromCity(i, routeStart);
                result.Add(new KeyValuePair<int, int[]>(optimizedRoute.First(), optimizedRoute));
            }

            IsCalculated = true;
            CalculatedRoutes = result;
            return result;
        }

        public int[] CalculateRoutesFromCity(int cityIndex)
        {
            return _algorithmNonDeterministic.CalculateRoutesFromCity(cityIndex);
        }
    }
}