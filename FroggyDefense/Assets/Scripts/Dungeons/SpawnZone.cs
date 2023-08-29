using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.Enemies;
using Pathfinder;

namespace FroggyDefense.Core
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField] private SpawnInfo[] spawns;                    // What can spawn.
        [SerializeField] private Vector2 spawnTimerRange;               // Range of times until the next spawn.
        [SerializeField] private Vector2Int spawnAmountRange;           // Range of how many units can spawn at a time. Will choose highest possible amount.
        [SerializeField] private bool automaticSpawning;                // Will continue to spawn in enemies on its own.
        [SerializeField] private int maxActiveSpawns = 10;              // Maximum amount of enemies able to be active at once.

        [SerializeField] private HashSet<GameObject> activeSpawnList;   // List of all active spawns. To check if an enemy is owned by this zone.
        public int ActiveSpawns => activeSpawnList.Count;               // How many spawns are currently active.

        [SerializeField] private List<Vector2> validSpawnTiles;         // List of all valid spawn tiles in the range.

        private float spawnTickTimer = 0;

        private void Start()
        {
            activeSpawnList = new HashSet<GameObject>();

            BuildValidSpawnTileList();

            spawnTickTimer = Random.Range(spawnTimerRange.x, spawnTimerRange.y);
            Enemy.EnemyDefeatedEvent += OnEnemyDefeated;
        }

        private void Update()
        {
            if (spawnTickTimer <= 0)
            {
                if (ActiveSpawns < maxActiveSpawns)
                {
                    int amount = Random.Range(spawnAmountRange.x, spawnAmountRange.y + 1);
                    if (ActiveSpawns + amount > maxActiveSpawns)
                    {
                        amount = maxActiveSpawns - ActiveSpawns;
                    }
                    for (int i = 0; i < amount; i++) {
                        Spawn();
                    }
                }
                spawnTickTimer = Random.Range(spawnTimerRange.x, spawnTimerRange.y);
            } else
            {
                spawnTickTimer -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Spawns in enemies
        /// </summary>
        public void Spawn()
        {
            Vector2 pos;
            if (GetSpawnPoint(out pos)) {
                //GameObject spawn = Instantiate(spawns[0].prefab, pos, Quaternion.identity);
                GameObject spawn = SpawnManager.instance.Spawn(spawns[0].prefab, Random.Range(spawns[0].levelRange.x, spawns[0].levelRange.y + 1), pos);
                activeSpawnList.Add(spawn);

                Enemy enemy = null;
                if ((enemy = spawn.GetComponent<Enemy>()) != null)
                {
                    enemy.spawner = this;
                }
            } else
            {
                Debug.Log($"Could not find valid spawn point.");
            }
        }

        /// <summary>
        /// Gets a random spawn point within the range of the spawn zone.
        /// Returns true if successful and false is failed.
        /// </summary>
        /// <returns></returns>
        private bool GetSpawnPoint(out Vector2 pos)
        {
            if (validSpawnTiles == null || validSpawnTiles.Count <= 0)
            {
                pos = new Vector2();
                return false;
            }

            int rand = Random.Range(0, validSpawnTiles.Count);
            pos = validSpawnTiles[rand];
            return true;
        }

        /// <summary>
        /// Builds the list of valid spawn tiles.
        /// </summary>
        /// <returns></returns>
        private void BuildValidSpawnTileList()
        {
            Vector2Int xRange = new Vector2Int(Mathf.CeilToInt(transform.position.x - transform.localScale.x / 2), Mathf.CeilToInt(transform.position.x + transform.localScale.x / 2));
            Vector2Int yRange = new Vector2Int(Mathf.CeilToInt(transform.position.y - transform.localScale.y / 2), Mathf.CeilToInt(transform.position.y + transform.localScale.y / 2));
            BoardManager board = GameManager.instance.BoardManager;

            validSpawnTiles = new List<Vector2>();

            for (int x = xRange.x; x < xRange.y; x++)
            {
                for (int y = yRange.x; y < yRange.y; y++)
                {
                    Vector2Int tilePos = new Vector2Int(x, y);
                    PathfinderTile tile = board.PathfinderMap[tilePos];
                    if (!(tile.isImpassable || tile.isWater || tile.isWall))
                    {
                        validSpawnTiles.Add(tilePos);
                    }
                }
            }
        }

        /// <summary>
        /// When an enemy spawned by this zone dies, remove it from the active count.
        /// </summary>
        /// <param name="args"></param>
        private void OnEnemyDefeated(EnemyEventArgs args)
        {
            // If the defeated enemy is owned by this spawn zone, remove it from the count.
            if (activeSpawnList.Contains(args.enemy.gameObject))
            {
                activeSpawnList.Remove(args.enemy.gameObject);
            }
        }

        /// <summary>
        /// Holds info for what kinds of enemies can spawn in this zone.
        /// </summary>
        [System.Serializable]
        private struct SpawnInfo
        {
            public GameObject prefab;
            public Vector2Int levelRange;
            
            public SpawnInfo (GameObject prefab, Vector2Int levelRange)
            {
                this.prefab = prefab;
                this.levelRange = levelRange;
            }
        }
    }
}