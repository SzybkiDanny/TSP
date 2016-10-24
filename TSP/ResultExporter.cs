using System;
using System.IO;
using System.Linq;
using TSP.Algorithm;
using TSP.Algorithm.Optimizations;
using TSP.Algorithm.Optimizations.MultipleStartLocalSearch;

namespace TSP
{
    public static class ResultExporter
    {
        public static string Folder { get; set; } = @"Result\";
        public static string FileExtension { get; set; } = ".txt";

        public static void Save(TspAlgorithmBase algorithm)
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            var path = $"{Folder}{algorithm.Name}{FileExtension}";

            using (var outputFile = new StreamWriter(path, false))
            {
                outputFile.WriteLine("Najkrótsza trasa:");
                outputFile.WriteLine(GetRouteText(algorithm.ShortestRoute.Key,
                    algorithm.ShortestRoute.Value,
                    algorithm.Routes.First(q => q.Key == algorithm.ShortestRoute.Key).Value));

                outputFile.WriteLine("Najdłuższa trasa:");
                outputFile.WriteLine(GetRouteText(algorithm.LongestRoute.Key,
                    algorithm.LongestRoute.Value,
                    algorithm.Routes.First(q => q.Key == algorithm.LongestRoute.Key).Value));

                outputFile.WriteLine(Environment.NewLine + "Średnia długość tras: " +
                                     algorithm.AverageRouteLength);
                if (algorithm is TspAlgorithmWithStopWatch)
                {
                    outputFile.WriteLine(GetStopwatchText((TspAlgorithmWithStopWatch) algorithm));
                }
            }
        }

        private static string GetRouteText(int vertex, double length, int[] values)
        {
            var result = "";
            result += $"Z wierzchołka: {vertex}{Environment.NewLine}";
            result += $"Długość: {length}{Environment.NewLine}";
            result += $"Przebieg: {string.Join(", ", values)}{Environment.NewLine}";
            result += $"****************************************{Environment.NewLine}";
            return result;
        }

        private static string GetStopwatchText(TspAlgorithmWithStopWatch algorithm)
        {
            var result = "";
            result += $"Czas optymalizacji wszystkich tras: {algorithm.GetTimeOptimalizationAllRoutes} ms {Environment.NewLine}";
            result += $"Minimalny czas optymalizacji: {algorithm.GetMinTimeOptimalization} ms {Environment.NewLine}";
            result += $"Średni czas optymalizacji:  { algorithm.GetAvgTimeOptimalization} ms {Environment.NewLine}";
            result += $"Maksymalny czas optymalizacji: { algorithm.GetMaxTimeOptimalization} ms {Environment.NewLine}";
            return result;
        }
    }
}