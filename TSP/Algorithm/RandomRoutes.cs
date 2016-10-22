using System;
using System.Collections.Generic;
using System.Linq;
using TSP.Interface;

namespace TSP.Algorithm
{
    public class RandomRoutes : TspAlgorithmBase, INonDeterministicAlgorithm
    {
        public RandomRoutes()
        {
            Name = "RandomRoutes";
        }

        public override IList<KeyValuePair<int, int[]>> CalculateRoutes(IDictionary<int, int>[] distances)
        {
            Distances = distances;

            var result = new List<KeyValuePair<int, int[]>>();

            for (var i = 0; i < distances.Count(); i++)
                result.Add(new KeyValuePair<int, int[]>(i, (CalculateRoutesFromCity(i))));

            IsCalculated = true;
            CalculatedRoutes = result;

            return result;
        }

        public int[] CalculateRoutesFromCity(int cityIndex)
        {
            var route = new List<int> { cityIndex };
            for (var k = 0; k < (RouteLengthLimit ?? Distances.Length) - 1; k++)
                route.Add(GetNextCity(route));
            route.Add(cityIndex);
            return route.ToArray();
        }

        private int GetNextCity(List<int> route)
        {
            var random = new Random();
            var city = int.MaxValue;
            do
            {
                city = random.Next(Distances.Length);
            } while (route.Contains(city));
            return city;
        }
    }
}