using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.Algorithm {
	public class RoutesComparer {

		public int RouteVertexCompare(int[] route1, int[] route2) {

			var result = 0;
			foreach(var i in route1) {
				if(route2.Contains(i))
					result++;
			}
			return result;
		}

		public int RouteEdgeCompare(int[] route1, int[] route2) {

			var result = 0;
			for(int i = 0; i < route1.Length - 1; i++) {
				for(int j = 0; j < route2.Length - 1; j++) {
					if(route2[j] == route1[i] && route2[j + 1] == route1[i + 1])
						result++;

				}
			}
			return result;
		}
	}
}
