using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP.Algorithm
{
    public abstract class TspAlgorithmBase
    {
        protected IList<KeyValuePair<int, int[]>> CalculatedRoutes;
        public IDictionary<int, int>[] Distances;
        public bool IsCalculated { get; protected set; }
        public int? RouteLengthLimit { get; set; }
        public string Name { get; protected set; }

        public IList<KeyValuePair<int, int[]>> Routes
        {
            get
            {
                AssertIsCalculated();
                return CalculatedRoutes;
            }
        }

        public IList<KeyValuePair<int, int>> RoutesLength
            =>
            Routes.Select(q => new KeyValuePair<int, int>(q.Key, CalculateRouteLength(q.Value)))
                .ToList();

        public KeyValuePair<int, int> ShortestRoute
        {
            get
            {
                AssertIsCalculated();
                return RoutesLength.OrderBy(r => r.Value).First();
            }
        }

        public KeyValuePair<int, int> LongestRoute
        {
            get
            {
                AssertIsCalculated();
                return RoutesLength.OrderByDescending(r => r.Value).First();
            }
        }

        public double AverageRouteLength
        {
            get
            {
                AssertIsCalculated();
                return RoutesLength.Average(r => r.Value);
            }
        }

        public abstract IList<KeyValuePair<int, int[]>> CalculateRoutes(
            IDictionary<int, int>[] distances);


        private void AssertIsCalculated()
        {
            if (!IsCalculated)
                throw new Exception();
        }

        protected int CalculateRouteLength(int[] route)
        {
            var routeLength = 0;

            for (var i = 1; i < route.Count(); i++)
                routeLength += Distances[route[i - 1]][route[i]];

            return routeLength;
        }
    }
}