using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core
{
    public class SpawnZone : MonoBehaviour
    {
        private int MAXIMUM_GET_SPAWN_POINT_TRIES = 20;                 // Maximum amount of times GetSpawnPoint will try to get a random point before failing.
        [SerializeField] private SpawnInfo[] spawns;                    // What can spawn.
        [SerializeField] private Vector2 spawnTimerRange;               // Range of times until the next spawn.
        [SerializeField] private Vector2Int spawnAmountRange;           // Range of how many units can spawn at a time. Will choose highest possible amount.
        [SerializeField] private bool automaticSpawning;                // Will continue to spawn in enemies on its own.
        [SerializeField] private int maxActiveSpawns = 10;              // Maximum amount of enemies able to be active at once.

        [SerializeField] private HashSet<GameObject> activeSpawnList;   // List of all active spawns. To check if an enemy is owned by this zone.
        public int ActiveSpawns => activeSpawnList.Count;               // How many spawns are currently active.

        private Vector2 xRange;         // Range of x values to spawn things in between,
        private Vector2 yRange;         // Range of y values to spawn things in between.

        private void Start()
        {
            activeSpawnList = new HashSet<GameObject>();

            xRange = new Vector2(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2);
            yRange = new Vector2(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2);

            //Enemy.EnemyDefeatedEvent += OnEnemyDefeated;
        }

        /// <summary>
        /// Spawns in enemies
        /// </summary>
        public void Spawn()
        {
            GameObject spawn = Instantiate(spawns[0].prefab, GetSpawnPoint(), Quaternion.identity);
            activeSpawnList.Add(spawn);
        }

        /// <summary>
        /// Gets a random spawn point within the range of the spawn zone.
        /// </summary>
        /// <returns></returns>
        private Vector2 GetSpawnPoint()
        {
            BoardManager board = GameManager.instance.BoardManager;
            Vector2Int spawnPos;

            int loops = 0;
            while (loops++ < MAXIMUM_GET_SPAWN_POINT_TRIES)
            {
                spawnPos = new Vector2Int(Mathf.RoundToInt(Random.Range(xRange.x, xRange.y)), Mathf.RoundToInt(Random.Range(yRange.x, yRange.y)));
                
                if (!(board.PathfinderMap[spawnPos].isWall || board.PathfinderMap[spawnPos].isWater))
                {
                    Debug.Log($"RETURNING SPAWN POINT {spawnPos}. ({board.PathfinderMap[spawnPos].Name}) (isWall = {board.PathfinderMap[spawnPos].isWall}) (isWater = {board.PathfinderMap[spawnPos].isWater}).");
                    return spawnPos;
                }
            }

            // Default case is to spawn the enemey in at the center of the spawn point.
            return transform.position;
        }

        ///// <summary>
        ///// When an enemy spawned by this zone dies, remove it from the active count.
        ///// </summary>
        ///// <param name="args"></param>
        //private void OnEnemyDefeated(EnemyEventArgs args)
        //{
        //    // If the defeated enemy is owned by this spawn zone, remove it from the count.
        //    if (activeSpawnList.Contains(args.enemy.gameObject))
        //    {
        //        activeSpawnList.Remove(args.enemy.gameObject);
        //        activeSpawns--;
        //    }
        //}

        /// <summary>
        /// Holds info for what kinds of enemies can spawn in this zone.
        /// </summary>
        [System.Serializable]
        private struct SpawnInfo
        {
            public GameObject prefab;
            
            public SpawnInfo (GameObject prefab)
            {
                this.prefab = prefab;
            }
        }
    }
}