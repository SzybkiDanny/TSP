﻿using System.Linq;

namespace TSP.Algorithm.RoutesComparer
{
    public class RoutesComparer
    {
        public int RouteVertexCompare(int[] route1, int[] route2)
        {
            var result = 0;
            for (var i = 0; i < route1.Length - 1; i++)
                if (route2.Contains(route1[i]))
                    result++;
            return result;
        }

        public int RouteEdgeCompare(int[] route1, int[] route2)
        {
            var result = 0;
            for (var i = 0; i < route1.Length - 1; i++)
                for (var j = 0; j < route2.Length - 1; j++)
                    if ((route2[j] == route1[i]) && (route2[j + 1] == route1[i + 1]))
                        result++;
            return result;
        }
    }
}