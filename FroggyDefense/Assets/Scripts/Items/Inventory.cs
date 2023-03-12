using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Items
{
    public class Inventory : MonoBehaviour, IInventory
    {
        private InventorySlot[] _inventory;             // The list of all slots in the inventory.

        public bool Add(Item item)
        {
            throw new System.NotImplementedException();
        }

        public bool Add(Item item, int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(Item item)
        {
            foreach (InventorySlot slot in _inventory)
            {
                if (slot.item.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public Item Get(int index)
        {
            throw new System.NotImplementedException();
        }

        public int GetCount(int index)
        {
            throw new System.NotImplementedException();
        }

        public int GetIndex(Item item)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(Item item)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(int index)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// A slot in the inventory that holds an item and how much of the item there are.
        /// </summary>
        private class InventorySlot
        {
            public Item item { get; private set; } = null;
            public int count { get; private set; } = 0;

            public InventorySlot()
            {
                item = null;
                count = 0;
            }

            /// <summary>
            /// Adds as much of the given item as possible. Returns the amount
            /// of the item that was added.
            /// </summary>
            /// <param name="_item"></param>
            /// <returns></returns>
            public int Add(Item _item)
            {
                if (item == null)
                {
                    item = _item;
                    count = 1;
                    return 1;
                }
                return 0;
            }

            /// <summary>
            /// Clears the inventory slot of the item and count.
            /// </summary>
            public void Clear()
            {
                item = null;
                count = 0;
            }
        }
    }
}