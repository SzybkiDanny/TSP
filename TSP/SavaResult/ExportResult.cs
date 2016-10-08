using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Algorithm;

namespace TSP.SavaResult
{
    public static class ExportResult
    {
        private static string folder = @"Result\";

        public static void Save(TspAlgorithmBase algorithm)
        {


            if (!Directory.Exists(folder))
            {

                DirectoryInfo di = Directory.CreateDirectory(folder);
            }

            string path = folder + algorithm.Name + ".txt";
           
            using (StreamWriter outputFile = new StreamWriter(path, false))
            {
              
                outputFile.WriteLine("Najkrótsza trasa:");
                outputFile.WriteLine(GetRouteText(algorithm.ShortestRoute.Key, algorithm.ShortestRoute.Value, algorithm.Routes.First(q => q.Key == algorithm.ShortestRoute.Key).Value));

                outputFile.WriteLine("Najdłuższa trasa:");
                outputFile.WriteLine(GetRouteText(algorithm.LongestRoute.Key, algorithm.LongestRoute.Value, algorithm.Routes.First(q => q.Key == algorithm.LongestRoute.Key).Value));

                outputFile.WriteLine(Environment.NewLine + "Średnia długość tras: " + algorithm.AverageRouteLength);
                
            }
        }
        private static string GetRouteText(int vertex,double length,int[] values)
        {
            string result = "";
            result += "Z wierzchołka: " + vertex + Environment.NewLine;
            result += "Długość: " + length + Environment.NewLine;
            result += "Przebieg: " + String.Join(",", values) + Environment.NewLine;
            result += "****************************************" + Environment.NewLine;
            return result;
        }
    }
}
