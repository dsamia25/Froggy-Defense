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
        public static List<Vector2> FindShortestPath(ICollection<Vector2Int> map, Vector2Int start, Vector2Int finish)
        {
            SortedDictionary<Vector2Int, GridNode> createdNodes = new SortedDictionary<Vector2Int, GridNode>();     // Keeps track of which node positions have already been created.
            Dictionary<GridNode, GridNode> index = new Dictionary<GridNode, GridNode>();                            // Index for each node and the node to come from for the quickets path.

            List<GridNode> unvisited = new List<GridNode>();                                                        // Which nodes have not been visited yet.

            GridNode startNode = new GridNode(start, start, finish);
            unvisited.Add(startNode);
            index.Add(startNode, null);

            do
            {
                GridNode curr = unvisited[0];
                unvisited.Remove(curr);

                CreateNodes(map, createdNodes, unvisited, curr, start, finish);
                MapNode(index, curr);

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
        /// Creates GridNodes using the input map and sets them as neighbors to the node.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="node"></param>
        private static void CreateNodes(ICollection<Vector2Int> map, IDictionary<Vector2Int, GridNode> createdNodes, List<GridNode> unvisited, GridNode curr, Vector2Int start, Vector2Int finish)
        {
            Vector2Int pos;             // Current position to look at.
            // Top Left
            pos = new Vector2Int(curr.Position.x - 1, curr.Position.y + 1);
            if (map.Contains(pos))
            {
                CreateNode(pos, createdNodes, unvisited, curr, start, finish);
            }
            // Top Mid
            pos = new Vector2Int(curr.Position.x, curr.Position.y + 1);
            if (map.Contains(pos))
            {
                CreateNode(pos, createdNodes, unvisited, curr, start, finish);
            }
            // Top Right
            pos = new Vector2Int(curr.Position.x + 1, curr.Position.y + 1);
            if (map.Contains(pos))
            {
                CreateNode(pos, createdNodes, unvisited, curr, start, finish);
            }
            // Left Mid
            pos = new Vector2Int(curr.Position.x - 1, curr.Position.y);
            if (map.Contains(pos))
            {
                CreateNode(pos, createdNodes, unvisited, curr, start, finish);
            }
            // Right Mid
            pos = new Vector2Int(curr.Position.x + 1, curr.Position.y);
            if (map.Contains(pos))
            {
                CreateNode(pos, createdNodes, unvisited, curr, start, finish);
            }
            // Bot Left
            pos = new Vector2Int(curr.Position.x - 1, curr.Position.y - 1);
            if (map.Contains(pos))
            {
                CreateNode(pos, createdNodes, unvisited, curr, start, finish);
            }
            // Bot Mid
            pos = new Vector2Int(curr.Position.x, curr.Position.y - 1);
            if (map.Contains(pos))
            {
                CreateNode(pos, createdNodes, unvisited, curr, start, finish);
            }
            // Bot Right
            pos = new Vector2Int(curr.Position.x + 1, curr.Position.y - 1);
            if (map.Contains(pos))
            {
                CreateNode(pos, createdNodes, unvisited, curr, start, finish);
            }
        }

        /// <summary>
        /// Checks if a node has already been created for the given position and creates it if not.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="createdNodes"></param>
        /// <param name="unvisited"></param>
        /// <param name="curr"></param>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        private static void CreateNode(Vector2Int pos, IDictionary<Vector2Int, GridNode> createdNodes, List<GridNode> unvisited, GridNode curr, Vector2Int start, Vector2Int finish)
        {
            if (!createdNodes.ContainsKey(pos))
            {
                GridNode node = new GridNode(pos, curr, finish);
                createdNodes.Add(pos, node);
                unvisited.Add(node);
                curr.ConnectedNodes.Add(node);
            }
        }

        /// <summary>
        /// Evaluates the node's neighbors and the quickest route to each.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="curr"></param>
        private static void MapNode(Dictionary<GridNode, GridNode> index, GridNode curr)
        {
            foreach (GridNode node in curr.ConnectedNodes)
            {
                if (index.ContainsKey(node))
                {
                    // Check if this path to an already discovered node is better.
                    if (curr.CompareTo(index[node]) < 0)
                    {
                        index[node] = curr;
                        node.UpdateValues(curr);
                    }
                } else
                {
                    index.Add(node, curr);
                }
            }
        }

        /// <summary>
        /// Returns the optimal path from start to end along the given node map.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static List<Vector2> BuildPath(IDictionary<GridNode, GridNode> index, GridNode start, GridNode end)
        {
            List<Vector2> path = new List<Vector2>();
            GridNode curr = end;
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
        private class GridNode: IComparable
        {
            public Vector2Int Position { get; private set; }                    // The node's position.
            public float StartDistance { get; private set; }                    // The node's distance from the starting position.
            public float EndDistance { get; private set; }                      // The node's distance from the ending position.
            public float Value { get; private set; }                            // The combined weight of distances.
            public List<GridNode> ConnectedNodes;                               // All of the connected nodes.

            public GridNode(Vector2Int pos, Vector2Int startPos, Vector2Int endPos)
            {
                ConnectedNodes = new List<GridNode>();
                Position = pos;
                StartDistance = Vector2Int.Distance(Position, startPos);
                EndDistance = Vector2Int.Distance(Position, endPos);
                Value = StartDistance + EndDistance;
            }

            public GridNode(Vector2Int pos, GridNode reference, Vector2Int endPos)
            {
                ConnectedNodes = new List<GridNode>();
                Position = pos;
                StartDistance = reference.StartDistance + Vector2Int.Distance(Position, reference.Position);
                EndDistance = Vector2Int.Distance(Position, endPos);
                Value = StartDistance + EndDistance;
            }

            /// <summary>
            /// Updates all of the values using the new reference node.
            /// </summary>
            /// <param name="node"></param>
            public void UpdateValues(GridNode node)
            {
                StartDistance = node.StartDistance + Vector2Int.Distance(Position, node.Position);
                Value = StartDistance + EndDistance;
            }

            public int CompareTo(object obj)
            {
                GridNode otherNode = obj as GridNode;
                if (this.Value < otherNode.Value)
                {
                    return -1;
                } else if (this.Value > otherNode.Value)
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