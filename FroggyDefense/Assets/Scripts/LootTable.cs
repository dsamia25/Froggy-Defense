using System;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Core
{
    /// <summary>
    /// Table of items that can be dropped.
    /// </summary>
    [Serializable]
    public class LootTable
    {
        public LootTableItem[] Items;

        /// <summary>
        /// Rolls to drop each item.
        /// </summary>
        /// <returns></returns>
        public List<ItemObject> Roll()
        {
            try
            {
                List<ItemObject> loot = new List<ItemObject>();
                foreach (LootTableItem item in Items)
                {
                    if (UnityEngine.Random.Range(0f, 1f) < item.dropChance)
                    {
                        loot.Add(item.item);
                    }
                }
                return loot;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error rolling for items: {e}");
                return new List<ItemObject>();
            }
        }
    }

    /// <summary>
    /// An item corresponding to its drop chance.
    /// </summary>
    [Serializable]
    public struct LootTableItem
    {
        public ItemObject item;
        public float dropChance;

        public LootTableItem(ItemObject _item, float _dropChance)
        {
            item = _item;
            dropChance = _dropChance;
        }
    }
}