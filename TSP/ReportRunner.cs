using System;
using System.Collections.Generic;
using TSP.Algorithm;
using TSP.Algorithm.Optimizations;
using TSP.Algorithm.Optimizations.Evolutionary;
using TSP.Algorithm.Optimizations.IteratedLocalSearch;
using TSP.Algorithm.Optimizations.MultipleStartLocalSearch;
using TSP.Algorithm.RoutesComparer;

namespace TSP
{
    public static class ReportRunner
    {
        public static void Report1(IDictionary<int, int>[] data)
        {
        }

        public static void Report2(IDictionary<int, int>[] data)
        {
            var gc = new GreedyCycle {RouteLengthLimit = 50};
            RunAlgorithm(gc, data);

            var gcg = new GreedyCycleGrasp {RouteLengthLimit = 50};
            RunAlgorithm(gcg, data);

            var nn = new NN {RouteLengthLimit = 50};
            RunAlgorithm(nn, data);

            var nng = new NNGrasp {RouteLengthLimit = 50};
            RunAlgorithm(nng, data);

            var rr = new RandomRoutes {RouteLengthLimit = 50};
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
        }

        public static void Report3(IDictionary<int, int>[] data)
        {
            var runnerMultipleStartLocalSearch =
                new RunnerMultipleStartLocalSearch(new GreedyCycleGrasp {RouteLengthLimit = 50})
                {
                    CountStartInsideMultipleStartLocalSearch = 1000,
                    CountStartMultipleStartLocalSearch = 10
                };
            RunAlgorithm(runnerMultipleStartLocalSearch, data);

            //var runnerIteratedLocalSearch =
            //    new RunnerIteratedLocalSearch(new GreedyCycleGrasp {RouteLengthLimit = 50})
            //    {
            //        CountStartIteratedLocalSearch = 10,
            //        DurationLimit = (int) runnerMultipleStartLocalSearch.GetAvgTimeOptimalization/1000
            //    };
            //RunAlgorithm(runnerIteratedLocalSearch, data);
        }

        public static void Report4(IDictionary<int, int>[] data)
        {
            var rrC = new RunnerRoutesComparer {CountRoutes = 1000};
            rrC.CalculateRoutes(data);
            rrC.CompareRoutes();
            ResultExporter.SaveRoutescomparisions(rrC);
        }


        private static void RunAlgorithm(TspAlgorithmBase algorithm, IDictionary<int, int>[] data)
        {
            Console.WriteLine(algorithm.ShortestRoute.Value);
            algorithm.CalculateRoutes(data);
            ResultExporter.Save(algorithm);
            Console.WriteLine("Zapisano:" + algorithm.Name);
        }

        public static void Report5(IDictionary<int, int>[] data)
        {
            var algorithmRuns = new List<Evolutionary>();

            for (var i = 0; i < 10; i++)
            {
                var evo = new Evolutionary(new RandomRoutes() { RouteLengthLimit = 50 })
                {
                    Name = $"evolutionary{i + 1}",
                    DurationLimit = 30,
                    PopulationSize = 20
                };

                evo.CalculateRoutes(data);

                algorithmRuns.Add(evo);

                ResultExporter.SaveEvolutionary(evo);
            }
        }
    }
}