using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using TSP.Algorithm;
using TSP.Algorithm.Optimizations;

namespace TSP
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var data = LoadData(@".\kroA100.xml");

            var gc = new GreedyCycle { RouteLengthLimit = 50 };
            RunAlgorithm(gc, data);

             var gcg = new GreedyCycleGrasp { RouteLengthLimit = 50 };
            RunAlgorithm(gcg, data);

            var nn = new NN { RouteLengthLimit = 50 };
            RunAlgorithm(nn, data);

            var nng = new NNGrasp { RouteLengthLimit = 50 };
            RunAlgorithm(nng, data);

            var rr = new RandomRoutes() { RouteLengthLimit = 50 };
            RunAlgorithm(rr, data);

            var lsGc = new LocalSearch(gc);
            RunAlgorithm(lsGc, data);

            var lsGcg = new LocalSearch(gcg);
            RunAlgorithm(lsGcg, data);

            var lsNn = new LocalSearch(nn);
            RunAlgorithm(lsNn, data);

            var lsNng = new LocalSearch(nng);
            RunAlgorithm(lsNng, data);

            var lsRr = new LocalSearch(rr);
            RunAlgorithm(lsRr, data);

            Console.WriteLine("Zakończono");
            Console.ReadKey(true);
        }

        private static void RunAlgorithm(TspAlgorithmBase algorithm, IDictionary<int, int>[] data)
        {
            algorithm.CalculateRoutes(data);
            Console.WriteLine(algorithm.ShortestRoute.Value);
            ResultExporter.Save(algorithm);
            Console.WriteLine("Zapisano:" + algorithm.Name);
        }
        private static IDictionary<int, int>[] LoadData(string path)
        {
            var doc = XDocument.Load(path);

            return
                doc.Root.Descendants("vertex").Select(
                    v => v.Descendants().ToDictionary(e => int.Parse(e.Value),
                        e => (int)Math.Round(double.Parse(e.Attribute("cost").Value, 
                                CultureInfo.InvariantCulture), 0,
                            MidpointRounding.AwayFromZero))).ToArray();
        }
    }
}