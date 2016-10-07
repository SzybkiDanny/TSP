using System;
using System.Collections.Generic;
using System.Linq;
using TSP.Rcl;

namespace TSP.Algorithm
{
    public class GreedyCycleGrasp : TspAlgorithmBase
    {
        private const int RestrictedCandidateListLimit = 3;

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
            var rcl = new RestrictedCandidateList(RestrictedCandidateListLimit);

            for (var k = 0; k < 48; k++)
            {
                for (var i = 0; i < Distances.Length; i++)
                {
                    if (route.Contains(i))
                        continue;

                    for (var j = 1; j < route.Count; j++)
                    {
                        if (route.Count == 3 && j == 2)
                            continue;

                        rcl.TryAdd(new Candidate()
                        {
                            CityIndex = i,
                            InsertAt = j,
                            Delta = Distances[route[j - 1]][i] + Distances[i][route[j]] -
                                Distances[route[j - 1]][route[j]]
                    });
                    }
                }

                var selectedCandidate = rcl.GetRandomCandidate();

                route.Insert(selectedCandidate.InsertAt, selectedCandidate.CityIndex);
                rcl.Empty();
            }

            return route.ToArray();
        }       
    }
}