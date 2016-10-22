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

            //ReportRunner.Report2(data);

            ReportRunner.Report3(data);

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