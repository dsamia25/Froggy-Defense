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
        public GameObject _testMarkerPrefab;

        public static BoardManager instance;

        public GameObject NexusHealthBarObject;

        [Space]
        [Header("Tiles")]
        [Space]
        public Tilemap[] Tilemaps;
        public TileBase[] tileSet;
        public float[] tileHeights = { 0, .3f, .8f };

        [Space]

        public Tilemap[] FullMap;               // The full version of the entire map.
        public GridTileObject[] MapLayerTiles;  // Which kind of tile is on each layer.
        public int expansionLength = 0;
        public int initialMapSize = 24;
        public int maxMapSize = 64;
        public List<Spawner> MoreSpawners = new List<Spawner>();    // More enemy spawners to be spawned in as the map expands.

        private IDictionary<Vector2Int, GridTile> PathinderMap;     // List of all tiles with their tile info for Pathfinder.

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

            TopBound = initialMapSize / 2;
            BotBound = -initialMapSize / 2;
            LeftBound = -initialMapSize / 2;
            RightBound = initialMapSize / 2;
        }

        private void Start()
        {
            // Create passable layers for test path.
            List<Tilemap> passableLayers = new List<Tilemap>();
            passableLayers.Add(Tilemaps[1]);    // Just grass layer for now.

            // Create test path
            List<Vector2> testPath = GridPathfinder.FindShortestPath(CreateTraversableMapView(passableLayers), new Vector2Int(-5, 9), Vector2Int.zero);

            // Make a tile adjustment because the path is going to the bottom right of each tile instead of the middle.
            float adjustment = Tilemaps[0].cellSize.x / 2;
            // Create visual test markers.
            foreach (Vector2 pos in testPath)
            {
                Vector2 adjustedPos = new Vector2(pos.x + adjustment, pos.y + adjustment);
                Instantiate(_testMarkerPrefab, adjustedPos, Quaternion.identity);
            }

            // Build PathfinderMap.
            // Do this after the map has been built or refresh each time it is changed for accurate pathfinding.
            BuildPathfinderMap();
        }

        #region Pathfinding
        // TODO: Test if this works, replace the traverablemapView one, make pathfinder use this instead of a list, make this find connectedtiles.
        /// <summary>
        /// Builds the pathfinder map using the tilemap layers.
        /// </summary>
        private void BuildPathfinderMap()
        {
            SortedDictionary<Vector2Int, GridTile> map = new SortedDictionary<Vector2Int, GridTile>();

            // Build nodes.
            for (int i = 0; i < FullMap.Length; i++)
            {
                Tilemap layer = FullMap[i];
                layer.CompressBounds();
                for (int x = Mathf.FloorToInt(layer.localBounds.min.x); x < Mathf.FloorToInt(layer.localBounds.max.x); x++)
                {
                    for (int y = Mathf.FloorToInt(layer.localBounds.min.y); y < Mathf.FloorToInt(layer.localBounds.max.y); y++)
                    {
                        if (layer.HasTile(new Vector3Int(x, y)))
                        {
                            Vector2Int pos = new Vector2Int(x, y);
                            GridTile tile = new GridTile(pos, MapLayerTiles[i]);
                            map.Add(pos, tile);
                        }
                    }
                }
            }

            // Assign the map.
            PathinderMap = map;

            // Connect nodes.
            foreach (var tilePos in map.Keys)
            {
                Vector2Int pos = Vector2Int.zero;

                // Top Left
                pos = new Vector2Int(tilePos.x - 1, tilePos.x + 1);
                if (map.ContainsKey(pos))
                {
                    map[tilePos].Connect(map[pos]);
                }

                // Top Mid
                pos = new Vector2Int(tilePos.x, tilePos.x + 1);
                if (map.ContainsKey(pos))
                {
                    map[tilePos].Connect(map[pos]);
                }

                // Top Right
                pos = new Vector2Int(tilePos.x + 1, tilePos.x + 1);
                if (map.ContainsKey(pos))
                {
                    map[tilePos].Connect(map[pos]);
                }

                // Left Mid
                pos = new Vector2Int(tilePos.x - 1, tilePos.x);
                if (map.ContainsKey(pos))
                {
                    map[tilePos].Connect(map[pos]);
                }

                // Right Mid
                pos = new Vector2Int(tilePos.x + 1, tilePos.x);
                if (map.ContainsKey(pos))
                {
                    map[tilePos].Connect(map[pos]);
                }

                // Bot Left
                pos = new Vector2Int(tilePos.x - 1, tilePos.x - 1);
                if (map.ContainsKey(pos))
                {
                    map[tilePos].Connect(map[pos]);
                }

                // Bot Mid
                pos = new Vector2Int(tilePos.x, tilePos.x - 1);
                if (map.ContainsKey(pos))
                {
                    map[tilePos].Connect(map[pos]);
                }

                // Bot Right
                pos = new Vector2Int(tilePos.x + 1, tilePos.x - 1);
                if (map.ContainsKey(pos))
                {
                    map[tilePos].Connect(map[pos]);
                }
            }
        }

        /// <summary>
        /// Merges each tilemap into a flat collection of traversable tiles.
        /// </summary>
        /// <param name="layers"></param>
        /// <returns></returns>
        public static ICollection<Vector2Int> CreateTraversableMapView(List<Tilemap> layers)
        {
            List<Vector2Int> map = new List<Vector2Int>();

            foreach (Tilemap layer in layers)
            {
                layer.CompressBounds();
                for (int x = Mathf.FloorToInt(layer.localBounds.min.x); x < Mathf.FloorToInt(layer.localBounds.max.x); x++)
                {
                    for (int y = Mathf.FloorToInt(layer.localBounds.min.y); y < Mathf.FloorToInt(layer.localBounds.max.y); y++)
                    {
                        if (layer.HasTile(new Vector3Int(x, y)))
                        {
                            map.Add(new Vector2Int(x, y));
                        }
                    }
                }
            }

            return map;
        }
        #endregion

        /// <summary>
        /// Randomly generates a level using LevelGenerator.
        /// </summary>
        public void BuildLevel()
        {
            if (RandomizeSeed)
            {
                do
                {
                    Seed = UnityEngine.Random.Range(-10000, 10000);
                } while (Seed % 10 == 0);
            }

            // Generate noise map with noise corresponding to height.
            float[,] heightMap = LevelGenerator.GenerateNoise(LevelWidth, LevelHeight, Seed, Scale);

            // Despawn old Nexus.
            if (Nexus != null)
            {
                Destroy(Nexus);
            }

            // Find Nexus spawn position.
            if (RandomizeNexusSpawn)
            {
                NexusSpawnLoc = new Vector2Int(
                    Mathf.RoundToInt(UnityEngine.Random.Range(NexusSpawnDistanceFromLedge * LevelWidth, (1 - NexusSpawnDistanceFromLedge) * LevelWidth)),
                    Mathf.RoundToInt(UnityEngine.Random.Range(NexusSpawnDistanceFromLedge * LevelWidth, (1 - NexusSpawnDistanceFromLedge) * LevelWidth))
                    );
            }

            // Set safe zone around Nexus.
            LevelGenerator.SetArea(
                heightMap,
                new Vector2Int(NexusSpawnLoc.x - NexusBufferZoneWidth / 2, NexusSpawnLoc.x + NexusBufferZoneWidth / 2),
                new Vector2Int(NexusSpawnLoc.y - NexusBufferZoneHeight / 2, NexusSpawnLoc.y + NexusBufferZoneHeight / 2),
                tileHeights[1]);

            // Spawn new Nexus.
            Nexus = Instantiate(Nexus_Prefab, new Vector2(NexusSpawnLoc.x, NexusSpawnLoc.y), Quaternion.identity);

            // Convert height map to tiles.
            int[,] newMap = LevelGenerator.InterpretNoise(heightMap, tileHeights);

            // Spawn in tiles.
            LevelGenerator.RenderMap(newMap, Tilemaps, tileSet);
        }

        public void InitBoardSize()
        {
            ExpandLevel(initialMapSize / 2);
        }

        public void ExpandLevel()
        {
            ExpandLevel(expansionLength);
        }

        /// <summary>
        /// Expands the level by enabling adding the next set of tiles to the
        /// main tilemaps from the expansion tilemaps.
        /// </summary>
        public void ExpandLevel(int expoLength)
        {
            // TODO: Maybe get rid of this check for randomly generated maps.
            // Don't expand past the max size of the map.
            if (TopBound + expoLength > maxMapSize) return;

            int newTopBound = TopBound + expoLength;
            int newBotBound = BotBound - expoLength;
            int newLeftBound = LeftBound - expoLength;
            int newRightBound = RightBound + expoLength;

            for (int x = newLeftBound; x < newRightBound; x++)
            {
                for (int y = newBotBound; y < newTopBound; y++)
                {
                    if (x > LeftBound && x < RightBound && y > BotBound && y < TopBound) continue;
                    for (int i = 0; i < Tilemaps.Length; i++)
                    {
                        Tilemap mainMap = Tilemaps[i];
                        Tilemap expoMap = FullMap[i];
                        Vector3Int pos = new Vector3Int(x, y, 0);
                        mainMap.SetTile(pos, expoMap.GetTile(pos));
                    }
                }
            }

            TopBound = newTopBound;
            BotBound = newBotBound;
            LeftBound = newLeftBound;
            RightBound = newRightBound;

            EnableLevelExpansionSpawners();
        }

        // TODO: Make a randomized way to spawn in a set amount of spawners at random locations in the new expansion zone.
        /// <summary>
        /// Enables spawners from the MoreSpawners list once the tiles they are over become available.
        /// </summary>
        public void EnableLevelExpansionSpawners()
        {
            List<Spawner> enabledSpawners = new List<Spawner>();

            foreach(Spawner spawner in MoreSpawners)
            {
                // If the position under the spawner is active then spawn the spawner in and add it to the GameManager's list.
                if (Tilemaps[1].GetTile(Tilemaps[1].WorldToCell(spawner.transform.position)) != null)
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
    }
}
