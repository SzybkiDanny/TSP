using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TSP.Algorithm.Optimizations.Evolutionary
{
    public class Evolutionary : TspAlgorithmBase
    {
        private readonly TspAlgorithmBase _randomAlgorithm;
        private readonly Random _random = new Random();

        public int PopulationSize { get; set; }
        public int DurationLimit { get; set; }

        public long Duration { get; set; }

        public Evolutionary(TspAlgorithmBase algorithm)
        {
            Name = "Evolutionary";
            _randomAlgorithm = algorithm;
            RouteLengthLimit = _randomAlgorithm.RouteLengthLimit;

        }

        public List<long> LSRuntimes { get; set; } = new List<long>();
        public List<int> LSResults { get; set; } = new List<int>();

        public override IList<KeyValuePair<int, int[]>> CalculateRoutes(
            IDictionary<int, int>[] distances)
        {
            Distances = distances;

            var result = _randomAlgorithm.CalculateRoutes(distances).Take(PopulationSize).ToList();
            var stopwatch = new Stopwatch();
            var longestRouteIndex = GetLongestRouteIndex(result);
            var localSearch = new LocalSearch();
            var lsStopwatch = new Stopwatch();

            localSearch.Distances = Distances;

            LSRuntimes.Clear();
            LSResults.Clear();

            stopwatch.Start();

            do
            {
                var firstRouteIndex = _random.Next(PopulationSize);
                var secondRouteIndex = _random.Next(PopulationSize);

                while (secondRouteIndex == firstRouteIndex)
                    secondRouteIndex = _random.Next(PopulationSize);

                var recombinedRoute = RecombineRoutes(result[firstRouteIndex].Value, result[secondRouteIndex].Value);
                lsStopwatch.Restart();
                recombinedRoute = localSearch.OptimizeRouteFromCity(recombinedRoute.First(),
                    recombinedRoute);
                lsStopwatch.Stop();
                LSRuntimes.Add(lsStopwatch.ElapsedMilliseconds);
                var recombinedRouteLength = CalculateRouteLength(recombinedRoute);
                LSResults.Add(recombinedRouteLength);

                if (recombinedRouteLength >= CalculateRouteLength(result[longestRouteIndex].Value))
                    continue;

                if (!result.TrueForAll(x => !x.Value.Contains(recombinedRoute)))
                    continue;

                result[longestRouteIndex] = new KeyValuePair<int, int[]>(recombinedRoute.First(), recombinedRoute);
                longestRouteIndex = GetLongestRouteIndex(result);

            } while (stopwatch.Elapsed <= TimeSpan.FromSeconds(DurationLimit));
            Duration = stopwatch.ElapsedMilliseconds;

            IsCalculated = true;
            CalculatedRoutes = result;

            return result;
        }

        private int GetLongestRouteIndex(IList<KeyValuePair<int, int[]>> routes)
        {
            var longestRouteLength = int.MinValue;
            var longestRouteIndex = 0;

            for (var i = 0; i < routes.Count; i++)
            {
                var routeLength = CalculateRouteLength(routes[i].Value);

                if (routeLength <= longestRouteLength)
                    continue;
                longestRouteIndex = i;
                longestRouteLength = routeLength;
            }

            return longestRouteIndex;
        }

        private int[] RecombineRoutes(int[] firstRoute, int[] secondRoute)
        {
            firstRoute = firstRoute.Reverse().Skip(1).Reverse().ToArray();
            secondRoute = secondRoute.Reverse().Skip(1).Reverse().ToArray();

            var similarParts = GetSimilarParts(firstRoute, secondRoute);
            var similarVerticesCount = similarParts.Sum(part => part.Length);

            while (similarVerticesCount++ < RouteLengthLimit)
            {
                var newVertex = _random.Next(Distances.Length);

                while (similarParts.Any(part => part.Contains(newVertex)))
                    newVertex = _random.Next(Distances.Length);

                similarParts.Add(new[] {newVertex});
            }

            return MergeSimilarParts(similarParts);
        }

        private List<int[]> GetSimilarParts(int[] firstRoute, int[] secondRoute)
        {
            var result = new List<int[]>();
            var minLength = Math.Min(firstRoute.Length, secondRoute.Length);

            for (var i = minLength; i > 0; i--)
            {
                for (var j = 0; j < secondRoute.Length - i + 1; j++)
                {
                    if (firstRoute.Length < i)
                        break;

                    var routePart = secondRoute.ToList().GetRange(j, i).ToArray();
                    var startIndexToRemove = -1;

                    if (firstRoute.Contains(routePart))
                        startIndexToRemove =
                            firstRoute.ToList().FindIndex(x => x == routePart.First());
                    else if (firstRoute.Contains(routePart.Reverse()))
                        startIndexToRemove =
                            firstRoute.ToList().FindIndex(x => x == routePart.Reverse().First());

                    if (startIndexToRemove < 0)
                        continue;

                    result.Add(routePart);

                    var newRoute = firstRoute.ToList();
                    newRoute.RemoveRange(startIndexToRemove, i);
                    firstRoute = newRoute.ToArray();
                }
            }

            return result;
        }

        private int[] MergeSimilarParts(List<int[]> similarParts)
        {
            var result = new List<int>();

            while (similarParts.Count > 0)
            {
                var partToAddIndex = _random.Next(similarParts.Count);
                var partToAdd = similarParts[partToAddIndex].ToList();

                if (_random.Next(2) == 0)
                    partToAdd.Reverse();

                result.AddRange(partToAdd);
                similarParts.RemoveAt(partToAddIndex);
            }

            result.Add(result.First());

            return result.ToArray();
        }
    }

    public static class EnumerableExtensions
    {
        public static bool Contains<T>(this IEnumerable<T> data, IEnumerable<T> otherData)
        {
            var dataLength = data.Count();
            var otherDataLength = otherData.Count();

            if (dataLength < otherDataLength)
                return false;

            return Enumerable.Range(0, dataLength - otherDataLength + 1)
                .Any(skip => data.Skip(skip).Take(otherDataLength).SequenceEqual(otherData));
        }
    }
}