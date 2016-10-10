﻿using System;
using System.Collections.Generic;
using System.Globalization;
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

            var gc = new GreedyCycle { RouteLengthLimit = 50 };
            gc.CalculateRoutes(data);
            ResultExporter.Save(gc);
            Console.WriteLine("Zapisano:" + gc.Name);

            var gcg = new GreedyCycleGrasp { RouteLengthLimit = 50 };
            gcg.CalculateRoutes(data);
            ResultExporter.Save(gcg);
            Console.WriteLine("Zapisano:" + gcg.Name);

            var nn = new NN { RouteLengthLimit = 50 };
            nn.CalculateRoutes(data);
            ResultExporter.Save(nn);
            Console.WriteLine("Zapisano:" + nn.Name);

            var nng = new NNGrasp { RouteLengthLimit = 50 };
            nng.CalculateRoutes(data);
            ResultExporter.Save(nng);
            Console.WriteLine("Zapisano:" + nng.Name);

            Console.WriteLine("Zakończono");
            Console.ReadKey(true);
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