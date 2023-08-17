using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinder
{
    // TODO: Find a better way to handle layers. Maybe use LayerMask or an equivalent bit masking system instead?
    public struct LayerInfo
    {
        public bool includeWater;
        public bool includeWalls;

        public LayerInfo(bool water, bool walls)
        {
            includeWater = water;
            includeWalls = walls;
        }
    }

    public static class TilePathfinder
    {
        /// <summary>
        /// Finds the shortest path from start to finish in the specified 2D map.
        /// Takes a map of each location with a tile and the tile's info.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        public static List<Vector2> FindShortestPath(IDictionary<Vector2Int, PathfinderTile> map, Vector2Int start, Vector2Int finish, LayerInfo layers)
        {
            try
            {
                if (map == null)
                {
                    throw new NullReferenceException("Map cannot be null.");
                }

                if (map.Count == 0)
                {
                    throw new ArgumentException("Map is empty.");
                }

                if (!(map.ContainsKey(start) && map.ContainsKey(finish)))
                {
                    throw new ArgumentException($"Either the start ({start}) or the finish ({finish}) is not in the map.");
                }

                if (layers.Equals(null))
                {
                    layers = new LayerInfo(false, false);
                }

                Dictionary<Vector2Int, TileNode> createdNodes = new Dictionary<Vector2Int, TileNode>();                 // Keeps track of which node positions have already been created.
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

                    // Check each node in main map.
                    foreach (PathfinderTile connected in map[curr.Position].ConnectedTiles)
                    {
                        // Check if the tile is traversable.
                        if (!IsTraversable(connected, layers))
                        {
                            continue;
                        }

                        // Check if node note created yet.
                        if (!createdNodes.ContainsKey(connected.Pos))
                        {
                            TileNode newNode = new TileNode(connected.Pos, curr, finish, map[connected.Pos]);
                            createdNodes.Add(connected.Pos, newNode);
                            unvisited.Add(newNode);
                            index.Add(newNode, curr);
                            continue;
                        }

                        // Compare current node to the linked one.
                        var node = createdNodes[connected.Pos];
                        var indexNode = index[node];
                        if (indexNode != null)
                        {
                            var currDis = Vector2Int.Distance(curr.Position, node.Position) * (curr.TileInfo.FromDistance + node.TileInfo.ToDistance) + curr.StartDistance;
                            var indexDis = Vector2Int.Distance(indexNode.Position, node.Position) * (indexNode.TileInfo.FromDistance + node.TileInfo.ToDistance) + indexNode.StartDistance;
                            if (currDis < indexDis)
                            {
                                index[node] = curr;
                                node.UpdateValues(curr);
                            }
                        }
                    }

                    unvisited.Sort((x, y) => { return x.CompareTo(y); });

                    // Check if found end node.
                    if (curr.Position == finish)
                    {
                        return BuildPath(index, createdNodes[start], createdNodes[finish]);
                    }
                } while (unvisited.Count > 0);

                return new List<Vector2>(); // Return empty set if no path.
            } catch (Exception e)
            {
                Debug.LogWarning($"Error finding path. {e}");
                return new List<Vector2>();
            }
        }

        /// <summary>
        /// Checks if the tile can be traversed with the allowed layers.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="traversableLayers"></param>
        /// <returns></returns>
        private static bool IsTraversable(PathfinderTile tile, LayerInfo traversableLayers)
        {
            return (!tile.isWater || (tile.isWater && traversableLayers.includeWater)) && (!tile.isWall || (tile.isWall && traversableLayers.includeWalls));
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
        /// Makes a string list of each point on the path with one point per line.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathToString(List<Vector2> path)
        {
            string str = "{\n";
            for (int i = 0; i < path.Count; i++)
            {
                str += "\t" + path[i].ToString() + (i < path.Count - 1 ? "," : "") + "\n";
            }
            str += "}";
            return str;
        }

        private static string PathIndexToString(IDictionary<TileNode, TileNode> index)
        {
            if (index == null) return "{\n}";

            string str = "{\n";
            foreach (var pair in index)
            {
                str += $"{(pair.Key != null ? pair.Key.Position : "null")} -> {(pair.Value != null ? pair.Value.Position : "null")}\n";
            }
            str += "}";
            return str;
        }

        /// <summary>
        /// Builds a map of nodes using the input tilemap layers and their corresponding node info.
        /// The array of layerTileInfo needs to be the same size as the tilemapLayers array so each layer
        /// has a corresponding entry.
        /// </summary>
        /// <param name="tilemapLayers"></param>
        /// <param name="layerTileInfo"></param>
        /// <returns></returns>
        public static Dictionary<Vector2Int, PathfinderTile> BuildNodeMap(Tilemap[] tilemapLayers, PathfinderTileObject[] layerTileInfo)
        {
            try
            {
                if (tilemapLayers.Length != layerTileInfo.Length)
                {
                    throw new ArgumentException("Input arrays must be the same length.");
                }

                Dictionary<Vector2Int, PathfinderTile> nodeMap = new Dictionary<Vector2Int, PathfinderTile>();

                // Build nodes.
                for (int i = tilemapLayers.Length - 1; i >= 0; i--)
                {
                    Tilemap layer = tilemapLayers[i];
                    layer.CompressBounds();
                    for (int x = Mathf.FloorToInt(layer.localBounds.min.x); x < Mathf.FloorToInt(layer.localBounds.max.x); x++)
                    {
                        for (int y = Mathf.FloorToInt(layer.localBounds.min.y); y < Mathf.FloorToInt(layer.localBounds.max.y); y++)
                        {
                            if (layer.HasTile(new Vector3Int(x, y)))
                            {
                                Vector2Int pos = new Vector2Int(x, y);
                                if (!nodeMap.ContainsKey(pos))
                                {
                                    PathfinderTile tile = new PathfinderTile(pos, layerTileInfo[i]);
                                    nodeMap.Add(pos, tile);
                                }
                            }
                        }
                    }
                }

                // Connect nodes.
                foreach (var tilePos in nodeMap.Keys)
                {
                    Vector2Int pos = Vector2Int.zero;

                    // Cycle through each connecting tile (Each tile one over in each direction).
                    for (int xOffset = -1; xOffset <= 1; xOffset += 1)
                    {
                        for (int yOffset = -1; yOffset <= 1; yOffset += 1)
                        {
                            // Skip connecting to self.
                            if (xOffset == 0 && yOffset == 0) continue;

                            pos = new Vector2Int(tilePos.x + xOffset, tilePos.y + yOffset);
                            if (nodeMap.ContainsKey(pos))
                            {
                                nodeMap[tilePos].Connect(nodeMap[pos]);
                            }
                        }
                    }
                }
                Debug.Log("Finished building node map.");

                // Return map.
                return nodeMap;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error building NodeMap: {e}");
                return null;
            }
        }

        /// <summary>
        /// A node that references its position and all connected nodes.
        /// </summary>
        private class TileNode : IComparable
        {
            public PathfinderTile TileInfo { get; private set; }                      // The tile info with travel values.
            public Vector2Int Position { get; private set; }                    // The node's position.
            public float StartDistance { get; private set; }                    // The node's distance from the starting position.
            public float EndDistance { get; private set; }                      // The node's distance from the ending position.
            public float Value { get; private set; }                            // The combined weight of distances.

            public TileNode(Vector2Int pos, Vector2Int startPos, Vector2Int endPos, PathfinderTile tile)
            {
                TileInfo = tile;
                Position = pos;
                StartDistance = Vector2Int.Distance(Position, startPos);
                EndDistance = Vector2Int.Distance(Position, endPos);
                Value = StartDistance + EndDistance;
            }

            public TileNode(Vector2Int pos, TileNode reference, Vector2Int endPos, PathfinderTile tile)
            {
                TileInfo = tile;
                Position = pos;
                StartDistance = Vector2Int.Distance(Position, reference.Position) * (reference.TileInfo.FromDistance + TileInfo.ToDistance) + reference.StartDistance;
                EndDistance = Vector2Int.Distance(Position, endPos);
                Value = StartDistance + EndDistance;
            }

            /// <summary>
            /// Updates all of the values using the new reference node.
            /// </summary>
            /// <param name="node"></param>
            public void UpdateValues(TileNode reference)
            {
                StartDistance = Vector2Int.Distance(Position, reference.Position) * (reference.TileInfo.FromDistance + TileInfo.ToDistance) + reference.StartDistance;
                Value = StartDistance + EndDistance;
            }

            public int CompareTo(object obj)
            {
                if (obj == null) return -1;

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