using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FroggyDefense.LevelGeneration;
using FroggyDefense.Core.Buildings;
using Pathfinder;

namespace FroggyDefense.Core
{
    public class BoardManager : MonoBehaviour
    {
        public LineRenderer lineRenderer;

        public GameObject _testMarkerPrefab;

        public static BoardManager instance;

        public GameObject NexusHealthBarObject;

        [Space]
        [Header("Tiles")]
        [Space]
        //public Tilemap[] Tilemaps;
        //public TileBase[] tileSet;
        //public float[] tileHeights = { 0, .3f, .8f };

        public Tilemap[] FullMap;                                           // The full version of the entire map.
        public PathfinderTileObject[] MapLayerTiles;                        // Which kind of tile is on each layer.
        public int expansionLength = 0;
        public int activeLevelRadius = 12;
        public int maxMapSize = 64;
        public List<Spawner> MoreSpawners = new List<Spawner>();            // More enemy spawners to be spawned in as the map expands.

        [HideInInspector]
        public IDictionary<Vector2Int, PathfinderTile> PathfinderMap;       // List of all tiles with their tile info for Pathfinder.

        [Space]
        [Header("Size")]
        [Space]
        public int LevelWidth = 100;            // Size of the map in tiles for random generation.
        public int LevelHeight = 100;           // Size of the map in tiles for random generation.
        public int TopBound { get; set; }       // Top Bound of the level in tile count.
        public int BotBound { get; set; }       // Bottom Bound of the level in tile count.
        public int LeftBound { get; set; }      // Left Bound of the level in tile count.
        public int RightBound { get; set; }     // Right Bound of the level in tile count.

        [Space]
        [Header("Nexus")]
        [Space]
        public GameObject Nexus = null;
        public GameObject Nexus_Prefab;
        public bool RandomizeNexusSpawn = false;
        public Vector2Int NexusSpawnLoc = Vector2Int.zero;
        public float NexusSpawnDistanceFromLedge = .1f;
        public int NexusBufferZoneWidth = 10;
        public int NexusBufferZoneHeight = 10;

        [Space]
        [Header("Seed")]
        [Space]
        public float Scale = 1;
        public int Seed = 1;
        public bool RandomizeSeed = false;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("More than one BoardManager");
                return;
            }
            instance = this;    // Set singleton

            TopBound = activeLevelRadius;
            BotBound = -activeLevelRadius;
            LeftBound = -activeLevelRadius;
            RightBound = activeLevelRadius;

            // Build PathfinderMap.
            // Do this after the map has been built or refresh each time it is changed for accurate pathfinding.
            PathfinderMap = TilePathfinder.BuildNodeMap(FullMap, MapLayerTiles);
            Debug.Log($"PathfinderMap size = {PathfinderMap.Count}.");
        }

        ///// <summary>
        ///// Randomly generates a level using LevelGenerator.
        ///// </summary>
        //public void BuildLevel()
        //{
        //    if (RandomizeSeed)
        //    {
        //        do
        //        {
        //            Seed = UnityEngine.Random.Range(-10000, 10000);
        //        } while (Seed % 10 == 0);
        //    }

        //    // Generate noise map with noise corresponding to height.
        //    float[,] heightMap = LevelGenerator.GenerateNoise(LevelWidth, LevelHeight, Seed, Scale);

        //    // Despawn old Nexus.
        //    if (Nexus != null)
        //    {
        //        Destroy(Nexus);
        //    }

        //    // Find Nexus spawn position.
        //    if (RandomizeNexusSpawn)
        //    {
        //        NexusSpawnLoc = new Vector2Int(
        //            Mathf.RoundToInt(UnityEngine.Random.Range(NexusSpawnDistanceFromLedge * LevelWidth, (1 - NexusSpawnDistanceFromLedge) * LevelWidth)),
        //            Mathf.RoundToInt(UnityEngine.Random.Range(NexusSpawnDistanceFromLedge * LevelWidth, (1 - NexusSpawnDistanceFromLedge) * LevelWidth))
        //            );
        //    }

        //    // Set safe zone around Nexus.
        //    LevelGenerator.SetArea(
        //        heightMap,
        //        new Vector2Int(NexusSpawnLoc.x - NexusBufferZoneWidth / 2, NexusSpawnLoc.x + NexusBufferZoneWidth / 2),
        //        new Vector2Int(NexusSpawnLoc.y - NexusBufferZoneHeight / 2, NexusSpawnLoc.y + NexusBufferZoneHeight / 2),
        //        tileHeights[1]);

        //    // Spawn new Nexus.
        //    Nexus = Instantiate(Nexus_Prefab, new Vector2(NexusSpawnLoc.x, NexusSpawnLoc.y), Quaternion.identity);

        //    // Convert height map to tiles.
        //    int[,] newMap = LevelGenerator.InterpretNoise(heightMap, tileHeights);

        //    // Spawn in tiles.
        //    LevelGenerator.RenderMap(newMap, Tilemaps, tileSet);
        //}

        //public void InitBoardSize()
        //{
        //    ExpandLevel(initialMapSize / 2);
        //}

        public void ExpandLevel()
        {
            activeLevelRadius += expansionLength;
            if (activeLevelRadius > maxMapSize)
            {
                activeLevelRadius = maxMapSize;
            }
            EnableLevelExpansionSpawners();
        }

        ///// <summary>
        ///// Expands the level by enabling adding the next set of tiles to the
        ///// main tilemaps from the expansion tilemaps.
        ///// </summary>
        //public void ExpandLevel(int expoLength)
        //{
        //    // TODO: Maybe get rid of this check for randomly generated maps.
        //    // Don't expand past the max size of the map.
        //    if (TopBound + expoLength > maxMapSize) return;

        //    int newTopBound = TopBound + expoLength;
        //    int newBotBound = BotBound - expoLength;
        //    int newLeftBound = LeftBound - expoLength;
        //    int newRightBound = RightBound + expoLength;

        //    // This is the system for expanding the current map using the full map as a template.
        //    //for (int x = newLeftBound; x < newRightBound; x++)
        //    //{
        //    //    for (int y = newBotBound; y < newTopBound; y++)
        //    //    {
        //    //        if (x > LeftBound && x < RightBound && y > BotBound && y < TopBound) continue;
        //    //        for (int i = 0; i < Tilemaps.Length; i++)
        //    //        {
        //    //            Tilemap mainMap = Tilemaps[i];
        //    //            Tilemap expoMap = FullMap[i];
        //    //            Vector3Int pos = new Vector3Int(x, y, 0);
        //    //            mainMap.SetTile(pos, expoMap.GetTile(pos));
        //    //        }
        //    //    }
        //    //}

        //    TopBound = newTopBound;
        //    BotBound = newBotBound;
        //    LeftBound = newLeftBound;
        //    RightBound = newRightBound;

        //    EnableLevelExpansionSpawners();
        //}

        // TODO: Make a randomized way to spawn in a set amount of spawners at random locations in the new expansion zone.
        /// <summary>
        /// Enables spawners from the MoreSpawners list once the tiles they are over become available.
        /// </summary>
        public void EnableLevelExpansionSpawners()
        {
            List<Spawner> enabledSpawners = new List<Spawner>();

            foreach (Spawner spawner in MoreSpawners)
            {
                // This is the old system of activating spawners once the map is expanded to be beneath them.
                //// If the position under the spawner is active then spawn the spawner in and add it to the GameManager's list.
                //if (Tilemaps[1].GetTile(Tilemaps[1].WorldToCell(spawner.transform.position)) != null)
                //{
                //    spawner.gameObject.SetActive(true);
                //    GameManager.instance.Spawners.Add(spawner);
                //    enabledSpawners.Add(spawner);
                //}
                if (InActiveLevel(spawner.transform.position))
                {
                    spawner.gameObject.SetActive(true);
                    GameManager.instance.Spawners.Add(spawner);
                    enabledSpawners.Add(spawner);
                }
            }
            
            // Remove any enabled from the list.
            foreach(Spawner spawner in enabledSpawners)
            {
                MoreSpawners.Remove(spawner);
            }
        }

        /// <summary>
        /// Checks if a position is within the active bounds of the level.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool InActiveLevel(Vector2 pos)
        {
            if (pos.x < -activeLevelRadius && pos.x > activeLevelRadius) return false;
            if (pos.y < -activeLevelRadius && pos.y > activeLevelRadius) return false;
            return true;
        }
    }
}
