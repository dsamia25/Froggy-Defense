using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinder {
    public class PathfinderTile
    {
        public PathfinderTileObject template { get; }                 // Tile properties.
        public Vector2Int Pos { get; set; }                     // The tile's position.
        public List<PathfinderTile> ConnectedTiles;                   // List of all connected tiles.

        public string Name => template.Name;                    // The kind of tile.
        public bool isWater => template.isWater;                // Water tile that most things cannot pass.
        public bool isWall => template.isWall;                  // Wall tile that most things cannot pass.
        public bool isImpassable => template.isImpassable;      // Impassable tile that nothing can pass.
        public float ToDistance => template.ToDistance;         // The cost of moving from this tile.
        public float FromDistance => template.FromDistance;     // The cost of moving to this tile.

        public PathfinderTile(Vector2Int pos, PathfinderTileObject template)
        {
            this.template = template;
            Pos = pos;
            ConnectedTiles = new List<PathfinderTile>();
        }

        /// <summary>
        /// Finds the cost of moving to this input adjacent tile.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float Distance(PathfinderTile other)
        {
            return FromDistance + other.ToDistance;
        }

        public void Connect(PathfinderTile other)
        {
            try
            {
                if (!this.ConnectedTiles.Contains(other))
                {
                    this.ConnectedTiles.Add(other);
                }

                if (!other.ConnectedTiles.Contains(this))
                {
                    other.ConnectedTiles.Add(this);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error connecting tiles. " + e.ToString()); ;
            }
        }
    }
}