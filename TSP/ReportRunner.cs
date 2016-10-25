using System;
using System.Collections.Generic;
using TSP.Algorithm;
using TSP.Algorithm.Optimizations;
using TSP.Algorithm.Optimizations.IteratedLocalSearch;
using TSP.Algorithm.Optimizations.MultipleStartLocalSearch;
using TSP.Interface;

namespace TSP {
	public static class ReportRunner {
		public static void Report1(IDictionary<int, int>[] data) {
		}

		public static void Report2(IDictionary<int, int>[] data) {
			var gc = new GreedyCycle { RouteLengthLimit = 50 };
			RunAlgorithm(gc, data);

			var gcg = new GreedyCycleGrasp { RouteLengthLimit = 50 };
			RunAlgorithm(gcg, data);

			var nn = new NN { RouteLengthLimit = 50 };
			RunAlgorithm(nn, data);

			var nng = new NNGrasp { RouteLengthLimit = 50 };
			RunAlgorithm(nng, data);

			var rr = new RandomRoutes { RouteLengthLimit = 50 };
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

		public static void Report3(IDictionary<int, int>[] data) {
			var runnerMultipleStartLocalSearch =
				new RunnerMultipleStartLocalSearch(new GreedyCycleGrasp { RouteLengthLimit = 50 }) {
					CountStartInsideMultipleStartLocalSearch = 1000,
					CountStartMultipleStartLocalSearch = 10
				};
			RunAlgorithm(runnerMultipleStartLocalSearch, data);

			var ils = new IteratedLocalSearch(new GreedyCycleGrasp { RouteLengthLimit = 50 }) {
				DurationLimit = (int)runnerMultipleStartLocalSearch.GetAvgTimeOptimalization / 1000
			};
			RunAlgorithm(ils, data);
		}
		public static void Report4(IDictionary<int, int>[] data) {

			var resultVertices = new List<KeyValuePair<int, int>[]>();
			var resultEdges = new List<KeyValuePair<int, int>[]>();

			var routes = new List<int[]>();
			var algorithm = new RandomRoutes() { RouteLengthLimit = 50 };
			var random = new Random();

			var algorithmLocalSearch = new LocalSearch(algorithm);

			for(var i = 0; i < 1000; i++) {
				var routeStart = algorithm.CalculateRoutesFromCity(random.Next(data.Length));
				var optimizedRoute = algorithmLocalSearch.OptimizeRouteFromCity(i, routeStart);

				routes.Add(optimizedRoute);
			}

			for(var i = 0; i < routes.Count; i++) {
				for(var j = i + 1; j < routes.Count; j++) {
					var comparer = new RoutesComparer();
					var countVertices = comparer.RouteVertexCompare(routes[i], routes[j]);
					var countEdges = comparer.RouteEdgeCompare(routes[i], routes[j]);
				}
			}

		}


		private static void RunAlgorithm(TspAlgorithmBase algorithm, IDictionary<int, int>[] data) {
			algorithm.CalculateRoutes(data);
			Console.WriteLine(algorithm.ShortestRoute.Value);
			ResultExporter.Save(algorithm);
			Console.WriteLine("Zapisano:" + algorithm.Name);
		}
	}
}