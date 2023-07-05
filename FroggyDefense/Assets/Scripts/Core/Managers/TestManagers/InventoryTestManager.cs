using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Interactables;

namespace FroggyDefense.Core.Items.Tests
{
    public class InventoryTestManager : MonoBehaviour
    {
        public GameObject _groundItemPrefab = null;
        public ItemObject _testItemStacking = null;
        public ItemObject _testItemNonStacking = null;

        /// <summary>
        /// Spawns a number of test items at the position.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="count"></param>
        /// <param name="canStack"></param>
        public GameObject SpawnTestItem(Vector2 pos, bool canStack)
        {
            GameObject item = Instantiate(_groundItemPrefab, pos, Quaternion.identity);
            item.GetComponent<GroundItem>().SetItem(Item.CreateItem(canStack ? _testItemStacking : _testItemNonStacking));
            return item;
        }

        /// <summary>
        /// Spawns an input number of test items at the position.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="count"></param>
        /// <param name="canStack"></param>
        public GameObject[] SpawnTestItems(Vector2 pos, int count, bool canStack)
        {
            GameObject[] list = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                list[i] = SpawnTestItem(pos, canStack);
            }
            return list;
        }

        /// <summary>
        /// Spawns a random number of test items at the position and returns how
        /// many were spawned.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="count"></param>
        /// <param name="canStack"></param>
        public GameObject[] SpawnTestItems(Vector2 pos, Vector2Int randomRange, bool canStack)
        {
            int amount = UnityEngine.Random.Range(randomRange.x, randomRange.y);
            return SpawnTestItems(pos, amount, canStack);
        }
    }
}