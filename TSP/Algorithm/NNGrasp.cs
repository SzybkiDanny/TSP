using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP.Algorithm
{
    public class NNGrasp : TspAlgorithmBase
    {
        private Random _random = new Random();

        public NNGrasp()
        {
            Name = "NNGrasp";
        }

        public override IDictionary<int, int[]> CalculateRoutes(IDictionary<int, int>[] distances)
        {
            Distances = distances;

            var result = new Dictionary<int, int[]>();

            for (var i = 0; i < distances.Count(); i++)
                result[i] = CalculateRoutesFromCity(i);

            IsCalculated = true;
            CalculatedRoutes = result;

            return result;
        }

        private int[] CalculateRoutesFromCity(int cityIndex)
        {
            var route = new List<int> {cityIndex};

            for (var k = 0; k < (RouteLengthLimit ?? Distances.Length) - 1; k++)
            {
                var cityToAdd = GetRandomNextCity(GetThreeNextCities(route));
                route.Add(cityToAdd);
            }

            route.Add(cityIndex);
            return route.ToArray();
        }

        private List<int> GetThreeNextCities(List<int> route)
        {
            return
                Distances[route.Last()].OrderBy(q => q.Value).Select(q => q.Key).Where(
                    p => !route.Contains(p)).Take(3).ToList();
        }

        private int GetRandomNextCity(List<int> list)
        {
            return list.ElementAt(_random.Next(list.Count));
        }
    }
}