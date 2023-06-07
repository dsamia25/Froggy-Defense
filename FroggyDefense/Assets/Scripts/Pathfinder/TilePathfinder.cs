using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinder
{
    public static class TilePathfinder
    {
        /// <summary>
        /// Finds the shortest path from start to finish in the specified 2D map.
        /// Takes a map of each location with a tile and the tile's info.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        public static List<Vector2> FindShortestPath(IDictionary<Vector2Int, GridTile> map, Vector2Int start, Vector2Int finish)
        {
            SortedDictionary<Vector2Int, TileNode> createdNodes = new SortedDictionary<Vector2Int, TileNode>();     // Keeps track of which node positions have already been created.
            Dictionary<TileNode, TileNode> index = new Dictionary<TileNode, TileNode>();                            // Index for each node and the node to come from for the quickets path.

            List<TileNode> unvisited = new List<TileNode>();                                                        // Which nodes have not been visited yet.

            TileNode startNode = new TileNode(start, start, finish);
            unvisited.Add(startNode);
            index.Add(startNode, null);

            do
            {
                TileNode curr = unvisited[0];
                unvisited.Remove(curr);

                CreateNodes(map, createdNodes, unvisited, curr, start, finish);
                //MapNode(index, curr);

                unvisited.Sort();

                // Check if found end node.
                if (curr.Position == finish)
                {
                    return BuildPath(index, createdNodes[start], createdNodes[finish]);
                }
            } while (unvisited.Count > 0);

            return new List<Vector2>(); // Return empty set if no path.
        }

        /// <summary>
        /// Creates TileNodes using the input map and sets them as neighbors to the node.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="node"></param>
        private static void CreateNodes(IDictionary<Vector2Int, GridTile> map, IDictionary<Vector2Int, TileNode> createdNodes, List<TileNode> unvisited, TileNode curr, Vector2Int start, Vector2Int finish)
        {
            Vector2Int pos;             // Current position to look at.
            
            pos = new Vector2Int(curr.Position.x - 1, curr.Position.y + 1);
            if (map.ContainsKey(pos))
            {
                //CreateNode(pos, createdNodes, unvisited, curr, start, finish);
            }

            foreach (var connectedTile in map[curr.Position].ConnectedTiles)
            {
                
            }
        }

        /// <summary>
        /// Returns the optimal path from start to end along the given node map.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static List<Vector2> BuildPath(IDictionary<TileNode, TileNode> index, TileNode start, TileNode end)
        {
            List<Vector2> path = new List<Vector2>();
            TileNode curr = end;
            path.Add(curr.Position);
            do
            {
                curr = index[curr];
                if (curr != null)
                {
                    path.Add(curr.Position);
                }
            } while (curr != start && curr != null);
            path.Reverse();
            return path;
        }

        /// <summary>
        /// A node that references its position and all connected nodes.
        /// </summary>
        private class TileNode : IComparable
        {
            public Vector2Int Position { get; private set; }                    // The node's position.
            public float StartDistance { get; private set; }                    // The node's distance from the starting position.
            public float EndDistance { get; private set; }                      // The node's distance from the ending position.
            public float Value { get; private set; }                            // The combined weight of distances.
            public List<TileNode> ConnectedNodes;                               // All of the connected nodes.

            public TileNode(Vector2Int pos, Vector2Int startPos, Vector2Int endPos)
            {
                ConnectedNodes = new List<TileNode>();
                Position = pos;
                StartDistance = Vector2Int.Distance(Position, startPos);
                EndDistance = Vector2Int.Distance(Position, endPos);
                Value = StartDistance + EndDistance;
            }

            public TileNode(Vector2Int pos, TileNode reference, Vector2Int endPos)
            {
                ConnectedNodes = new List<TileNode>();
                Position = pos;
                StartDistance = reference.StartDistance + Vector2Int.Distance(Position, reference.Position);
                EndDistance = Vector2Int.Distance(Position, endPos);
                Value = StartDistance + EndDistance;
            }

            /// <summary>
            /// Updates all of the values using the new reference node.
            /// </summary>
            /// <param name="node"></param>
            public void UpdateValues(TileNode node)
            {
                StartDistance = node.StartDistance + Vector2Int.Distance(Position, node.Position);
                Value = StartDistance + EndDistance;
            }

            public int CompareTo(object obj)
            {
                TileNode otherNode = obj as TileNode;
                if (this.Value < otherNode.Value)
                {
                    return -1;
                }
                else if (this.Value > otherNode.Value)
                {
                    return 1;
                }
                else if (this.EndDistance < otherNode.EndDistance)
                {
                    return -1;
                }
                else if (this.EndDistance > otherNode.EndDistance)
                {
                    return 1;
                }
                else if (this.StartDistance < otherNode.StartDistance)
                {
                    return -1;
                }
                else if (this.StartDistance > otherNode.StartDistance)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}