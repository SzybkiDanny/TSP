using System.Collections.Generic;
using System.Linq;

namespace TSP.Algorithm
{
    public class GreedyCycle : TspAlgorithmBase
    {
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
            var route = new List<int> {cityIndex, Distances[cityIndex].OrderBy(d => d.Value).First().Key, cityIndex};

            for (var k = 0; k < 48; k++)
            {
                var delta = int.MaxValue;
                var newCityIndex = 0;
                var insertCityAt = 0;

                for (var i = 0; i < Distances.Length; i++)
                {
                    if (route.Contains(i))
                        continue;

                    for (var j = 1; j < route.Count; j++)
                    {
                        var newDelta = Distances[route[j - 1]][i] + Distances[i][route[j]] -
                                       Distances[route[j - 1]][route[j]];

                        if (newDelta >= delta)
                            continue;

                        delta = newDelta;
                        newCityIndex = i;
                        insertCityAt = j;
                    }
                }
                route.Insert(insertCityAt, newCityIndex);
            }

            return route.ToArray();
        }
    }
}