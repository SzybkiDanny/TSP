using System.Collections.Generic;
using System.Linq;

namespace TSP.Algorithm
{
    public class NN : TspAlgorithmBase
    {
        public NN()
        {
            Name = "NN";
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
            var route = new List<int> { cityIndex };
            for (var k = 0; k < (RouteLengthLimit ?? Distances.Length) - 1; k++)
            {
                route.Add(GetNextCity(route));
            }
            route.Add(cityIndex);
            return route.ToArray();
        }

        private int GetNextCity(List<int> route)
        {
            return Distances[route.Last()].OrderBy(q => q.Value).Select(q => q.Key).First(p => !route.Contains(p));
        }
    }
}
