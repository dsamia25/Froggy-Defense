using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FroggyDefense.Dungeons
{
    public class DungeonLevel : MonoBehaviour
    {
        public GameObject obj;

        [SerializeField] Tilemap GroundTiles;
        [SerializeField] Tilemap WaterTiles;

        private List<Vector2> GroundSpawnPoints;
        private List<Vector2> WaterSpawnPoints;

        private void Awake()
        {
            GroundSpawnPoints = TilemapToList(GroundTiles);
            WaterSpawnPoints = TilemapToList(WaterTiles);
        }

        // TODO: NONE OF THIS IS BEING USED.
        /// <summary>
        /// Fills the list with new random spawn points.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void GetSpawnPoints(int amount, List<Vector2> list)
        {
            list.Clear();

            if (amount < 0 || amount > GroundSpawnPoints.Count)
            {
                return;
            }

            while (list.Count < amount)
            {
                int i = UnityEngine.Random.Range(0, list.Count);
                if (!list.Contains(GroundSpawnPoints[i]))
                {
                    list.Add(GroundSpawnPoints[i]);
                }
            }
        }

        /// <summary>
        /// Creates a list of every tile in the tilemap.
        /// </summary>
        /// <param name="tilemap"></param>
        /// <returns></returns>
        public List<Vector2> TilemapToList(Tilemap tilemap)
        {
            List<Vector2> list = new List<Vector2>();
            tilemap.CompressBounds();
            for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                {
                    if (tilemap.HasTile(new Vector3Int(x, y)))
                    {
                        list.Add(new Vector2(x, y));
                    }
                }
            }
            return list;
        }

        //************************************************************
        // TEST METHODS
        //************************************************************

        /// <summary>
        /// TEST METHOD
        /// </summary>
        public void SpawnStuff()
        {
            foreach (var point in WaterSpawnPoints)
            {
                Instantiate(obj, point, Quaternion.identity);
            }

            foreach (var point in GroundSpawnPoints)
            {
                Instantiate(obj, point, Quaternion.identity);
            }
        }
    }
}