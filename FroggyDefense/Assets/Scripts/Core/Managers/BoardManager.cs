using UnityEngine;
using UnityEngine.Tilemaps;
using FroggyDefense.LevelGeneration;

namespace FroggyDefense.Core
{
    public class BoardManager : MonoBehaviour
    {
        public static BoardManager instance;

        public GameObject NexusHealthBarObject;

        [Space]
        [Header("Tiles")]
        [Space]
        public Tilemap[] Tilemaps;
        public TileBase[] tileSet;
        public float[] tileHeights = { 0, .3f, .8f };

        [Space]

        public Tilemap[] TilemapExpansions;
        public int expansionLength = 0;
        public int initialBoardSize = 24;

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

            TopBound = initialBoardSize / 2;
            BotBound = -initialBoardSize / 2;
            LeftBound = -initialBoardSize / 2;
            RightBound = initialBoardSize / 2;
        }

        /// <summary>
        /// Randomly generates a level using LevelGenerator.
        /// </summary>
        public void BuildLevel()
        {
            if (RandomizeSeed)
            {
                do
                {
                    Seed = Random.Range(-10000, 10000);
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
                    Mathf.RoundToInt(Random.Range(NexusSpawnDistanceFromLedge * LevelWidth, (1 - NexusSpawnDistanceFromLedge) * LevelWidth)),
                    Mathf.RoundToInt(Random.Range(NexusSpawnDistanceFromLedge * LevelWidth, (1 - NexusSpawnDistanceFromLedge) * LevelWidth))
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
            ExpandLevel(initialBoardSize / 2);
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
                        Tilemap expoMap = TilemapExpansions[i];
                        Vector3Int pos = new Vector3Int(x, y, 0);
                        mainMap.SetTile(pos, expoMap.GetTile(pos));
                    }
                }
            }

            TopBound = newTopBound;
            BotBound = newBotBound;
            LeftBound = newLeftBound;
            RightBound = newRightBound;
        }
    }
}
