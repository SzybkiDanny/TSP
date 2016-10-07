using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TSP.Algorithm;

namespace TSP
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var data = LoadData(@".\kroA100.xml");

            var gc = new GreedyCycle {RouteLengthLimit = 50};
            gc.CalculateRoutes(data);

            var gcg = new GreedyCycleGrasp {RouteLengthLimit = 50};
            gcg.CalculateRoutes(data);
        }

        private static IDictionary<int, int>[] LoadData(string path)
        {
            var doc = XDocument.Load(path);

            return
                doc.Root.Descendants("vertex").Select(
                    v => v.Descendants().ToDictionary(e => int.Parse(e.Value),
                        e => (int) Math.Round(double.Parse(e.Attribute("cost").Value), 0,
                            MidpointRounding.AwayFromZero))).ToArray();
        }
    }
}