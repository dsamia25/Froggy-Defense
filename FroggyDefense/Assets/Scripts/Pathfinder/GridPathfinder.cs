using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinder
{
    public static class GridPathfinder
    {
        /// <summary>
        /// Finds the shortest path from start to finish in the specified 2D map.
        /// The bool map requires that 0 is passable and 1 is impassable.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        public static List<Vector2Int> FindShortestPath(bool[,] map, Vector2Int start, Vector2Int finish)
        {
            Dictionary<Vector2Int, int> nodeDistanceIndex = new Dictionary<Vector2Int, int>();
            List<Vector2Int> unvisitedNodes = new List<Vector2Int>();
            Vector2Int currNode = start;

            do
            {
                // Get adjacent nodes.

                // Add adjacent nodes to index.

                    // If already in index, replace if shorter path found.

                // Set current node to next in list.
            } while (currNode != finish || unvisitedNodes.Count > 0);

            return null;
        }

        private static List<Vector2Int> FindShortestPathRecurse()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A node that references its position and all connected nodes.
        /// </summary>
        public class GridNode
        {
            public Vector2Int Position;
            public float Distance;
            public List<GridNode> ConnectedNodes;

            public GridNode(Vector2Int pos)
            {
                Position = pos;
                Distance = -1;
                ConnectedNodes = new List<GridNode>();
            }
        }
    }
}