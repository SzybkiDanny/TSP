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
                    outputFile.WriteLine(Environment.NewLine + "Czas optymalizacji wszystkich tras: " +
                                    ((TspAlgorithmWithStopWatch)algorithm).GetTimeOptimalizationAllRoutes);
                    outputFile.WriteLine(Environment.NewLine + "Minimalny czas optymalizacji: " +
                                   ((TspAlgorithmWithStopWatch)algorithm).GetMinTimeOptimalization);
                    outputFile.WriteLine(Environment.NewLine + "Średni czas optymalizacji: " +
                                  ((TspAlgorithmWithStopWatch)algorithm).GetAvgTimeOptimalization);
                    outputFile.WriteLine(Environment.NewLine + "Maksymalny czas optymalizacji: " +
                                  ((TspAlgorithmWithStopWatch)algorithm).GetMaxTimeOptimalization);
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
    }
}