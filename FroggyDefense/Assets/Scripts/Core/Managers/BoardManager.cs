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
        public Tilemap[] tilemaps;
        public TileBase[] tileSet;
        public float[] tileHeights = { 0, .3f, .8f };

        [Space]
        [Header("Level Expansions")]
        [Space]
        public GameObject[] levelExpansions;
        public int levelExpansionIndex = 0;

        [Space]
        [Header("Size")]
        [Space]
        public int LevelWidth = 100;
        public int LevelHeight = 100;

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
            LevelGenerator.RenderMap(newMap, tilemaps, tileSet);
        }

        /// <summary>
        /// Expands the level by enabling the next set of tiles.
        /// </summary>
        public void ExpandLevel()
        {
            if (levelExpansionIndex < levelExpansions.Length)
            {
                levelExpansions[levelExpansionIndex++].SetActive(true);
            }
        }
    }
}
