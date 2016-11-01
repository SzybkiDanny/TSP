using System;
using System.Collections.Generic;
using TSP.Algorithm.Optimizations;

namespace TSP.Algorithm.RoutesComparer
{
    public class RunnerRoutesComparer : TspAlgorithmBase
    {
        private Random _random;
        public IList<KeyValuePair<int, int[]>> ResultVertices { get; set; }
        public IList<KeyValuePair<int, int[]>> ResultEdges { get; set; }

        public int CountRoutes { get; set; }


        public override IList<KeyValuePair<int, int[]>> CalculateRoutes(IDictionary<int, int>[] distances)
        {
            Distances = distances;
            var result = new List<KeyValuePair<int, int[]>>();

            _random = new Random();
            var algorithm = new RandomRoutes {RouteLengthLimit = 50};
            algorithm.Distances = distances;

            var algorithmLocalSearch = new LocalSearch(algorithm);
            algorithmLocalSearch.Distances = distances;

            for (var i = 0; i < CountRoutes; i++)
            {
                var routeStart = algorithm.CalculateRoutesFromCity(_random.Next(distances.Length));
                var optimizedRoute = algorithmLocalSearch.OptimizeRouteFromCity(i, routeStart);

                result.Add(new KeyValuePair<int, int[]>(i, optimizedRoute));
            }
            IsCalculated = true;
            CalculatedRoutes = result;

            return result;
        }

        public void CompareRoutes()
        {
            ResultVertices = new List<KeyValuePair<int, int[]>>();
            ResultEdges = new List<KeyValuePair<int, int[]>>();
            var comparer = new RoutesComparer();

            for (var i = 0; i < CalculatedRoutes.Count; i++)
            {
                var edgesComparisions = new List<int>();
                var verticesComparisions = new List<int>();
                for (var j = 0; j < CalculatedRoutes.Count; j++)
                {
                    var countVertices = comparer.RouteVertexCompare(CalculatedRoutes[i].Value, CalculatedRoutes[j].Value);
                    var countEdges = comparer.RouteEdgeCompare(CalculatedRoutes[i].Value, CalculatedRoutes[j].Value);
                    edgesComparisions.Add(countEdges);
                    verticesComparisions.Add(countVertices);
                }
                ResultEdges.Add(new KeyValuePair<int, int[]>(i, edgesComparisions.ToArray()));
                ResultVertices.Add(new KeyValuePair<int, int[]>(i, verticesComparisions.ToArray()));
            }
        }

        public double AverageEdges(int idRoute)
        {
            double sum = 0;
            for (var i = 0; i < CountRoutes - 1; i++)
                sum += ResultEdges[idRoute].Value[i];

            return Math.Round(sum/CountRoutes - 1, 2);
        }

        public double AverageVertices(int idRoute)
        {
            double sum = 0;
            for (var i = 0; i < CountRoutes - 1; i++)
                sum += ResultVertices[idRoute].Value[i];

            return Math.Round(sum/CountRoutes - 1, 2);
        }

        public int CommonVertices(int idRoute1, int idRoute2)
        {
            return ResultVertices[idRoute1].Value[idRoute2];
        }

        public int CommonEdges(int idRoute1, int idRoute2)
        {
            return ResultEdges[idRoute1].Value[idRoute2];
        }
    }
}