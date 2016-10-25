using System;
using System.Collections.Generic;
using TSP.Algorithm;
using TSP.Algorithm.Optimizations;
using TSP.Algorithm.Optimizations.IteratedLocalSearch;
using TSP.Algorithm.Optimizations.MultipleStartLocalSearch;

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

            var ils = new IteratedLocalSearch(new GreedyCycleGrasp {RouteLengthLimit = 50})
            {
                DurationLimit = (int) runnerMultipleStartLocalSearch.GetAvgTimeOptimalization/1000
            };
            RunAlgorithm(ils, data);
        }

        private static void RunAlgorithm(TspAlgorithmBase algorithm, IDictionary<int, int>[] data)
        {
            algorithm.CalculateRoutes(data);
            Console.WriteLine(algorithm.ShortestRoute.Value);
            ResultExporter.Save(algorithm);
            Console.WriteLine("Zapisano:" + algorithm.Name);
        }
    }
}