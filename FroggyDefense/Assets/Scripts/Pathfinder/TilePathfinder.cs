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

            TileNode startNode = new TileNode(start, start, finish, map[start]);
            createdNodes.Add(start, startNode);
            unvisited.Add(startNode);
            index.Add(startNode, null);

            do
            {
                TileNode curr = unvisited[0];
                unvisited.Remove(curr);

                MapNode(map, createdNodes, index, unvisited, curr, start, finish);

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
        /// Evaluates the node's neighbors and the quickest route to each.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="curr"></param>
        private static void MapNode(IDictionary<Vector2Int, GridTile> map, IDictionary<Vector2Int, TileNode> createdNodes, IDictionary<TileNode, TileNode> index, List<TileNode> unvisited, TileNode curr, Vector2Int start, Vector2Int finish)
        {
            // Look at each connected node, check if created and create it if not, check if this is a better path and update it if so.
            foreach (var connectedTile in curr.TileInfo.ConnectedTiles)
            {
                if (createdNodes.ContainsKey(connectedTile.Pos))
                {
                    // Check if this path to an already discovered node is better.
                    TileNode node = createdNodes[connectedTile.Pos];
                    if (curr.CompareTo(index[node]) < 0)
                    {
                        index[node] = curr;
                        node.UpdateValues(curr);
                    }
                } else
                {
                    // Create new node.
                    CreateNode(connectedTile.Pos, map, createdNodes, index, unvisited, curr, finish);
                }
            }
        }

        /// <summary>
        /// Checks if this node has already been created and then makes it if note. Updates all lists with the new node too.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="map"></param>
        /// <param name="createdNodes"></param>
        /// <param name="index"></param>
        /// <param name="unvisited"></param>
        /// <param name="curr"></param>
        /// <param name="finish"></param>
        private static void CreateNode(Vector2Int pos, IDictionary<Vector2Int, GridTile> map, IDictionary<Vector2Int, TileNode> createdNodes, IDictionary<TileNode, TileNode> index, List<TileNode> unvisited, TileNode curr, Vector2Int finish)
        {
            if (!createdNodes.ContainsKey(pos))
            {
                TileNode node = new TileNode(pos, curr, finish, map[pos]);
                createdNodes.Add(pos, node);
                unvisited.Add(node);
                index.Add(node, curr);

                // Connect nodes.
                curr.Connect(node);
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
            public GridTile TileInfo { get; private set; }                      // The tile info with travel values.
            public Vector2Int Position { get; private set; }                    // The node's position.
            public float StartDistance { get; private set; }                    // The node's distance from the starting position.
            public float EndDistance { get; private set; }                      // The node's distance from the ending position.
            public float Value { get; private set; }                            // The combined weight of distances.
            public List<TileNode> ConnectedNodes;                               // All of the connected nodes.

            public TileNode(Vector2Int pos, Vector2Int startPos, Vector2Int endPos, GridTile tile)
            {
                TileInfo = tile;
                ConnectedNodes = new List<TileNode>();
                Position = pos;
                StartDistance = Vector2Int.Distance(Position, startPos);
                EndDistance = Vector2Int.Distance(Position, endPos);
                Value = StartDistance + EndDistance;
            }

            public TileNode(Vector2Int pos, TileNode reference, Vector2Int endPos, GridTile tile)
            {
                TileInfo = tile;
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
                StartDistance = Vector2Int.Distance(Position, node.Position) * TileInfo.Distance(node.TileInfo);
                Value = StartDistance + EndDistance;
            }

            /// <summary>
            /// Connects two nodes together.
            /// </summary>
            /// <param name="other"></param>
            public void Connect(TileNode other)
            {
                try
                {
                    if (!this.ConnectedNodes.Contains(other))
                    {
                        this.ConnectedNodes.Add(other);
                    }

                    if (!other.ConnectedNodes.Contains(this))
                    {
                        other.ConnectedNodes.Add(this);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Error connecting nodes. " + e.ToString()); ;
                }
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