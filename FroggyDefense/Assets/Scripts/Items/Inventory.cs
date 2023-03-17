using UnityEngine;
using FroggyDefense.Core;
using System.Collections.Generic;

namespace FroggyDefense.Items
{
    public class Inventory : MonoBehaviour, IInventory
    {
        private Dictionary<Item, int> _inventory;       // The collection of all items in the inventory.
        public Dictionary<Item, int> m_Inventory { get => _inventory; } // TODO: make this readonly.

        public int Size { get => _inventory.Count; }

        public event IInventory.InventoryDelegate InventoryChangedEvent;

        private void Awake()
        {
            // TODO: Make a way to load a saved inventory instead of creating a new one.
            // Instantiate the inventory.
            InitInventory();
        }

        public void InitInventory()
        {
            _inventory = new Dictionary<Item, int>();
            InventoryChangedEvent?.Invoke();
        }

        public void Add(Item item, int amount)
        {
            try
            {
                if (item.IsStackable)
                {
                    // If the item is stackable, look for it in the dictionary.
                    if (_inventory.ContainsKey(item))
                    {
                        // If the item is already in the dictionary, add to the existing stack.
                        _inventory[item] += amount;
                    }
                    else
                    {
                        // If the item is not already in the dictionary, create a new entry.
                        _inventory.Add(item, amount);
                    }
                }
                else
                {
                    // If the item is not stackable, it will always need a new entry with a size of 1.
                    _inventory.Add(item, 1);
                }
            } catch
            {
                Debug.Log("Error adding item to inventory.");
            }
            InventoryChangedEvent?.Invoke();
        }

        public bool Subtract(Item item, int amount)
        {
            if (amount < 0) return false;

            if (_inventory.ContainsKey(item))
            {
                _inventory[item] -= amount;

                if (_inventory[item] <= 0)
                {
                    Remove(item);
                }
                InventoryChangedEvent?.Invoke();
                return true;
            }
            InventoryChangedEvent?.Invoke();
            return false;
        }

        public bool Remove(Item item)
        {
            InventoryChangedEvent?.Invoke();
            return _inventory.Remove(item);
        }

        public Item GetItem(int index)
        {
            if (index < 0 || index >= _inventory.Count) return null;

            // TODO: Find a better way to do this.
            int count = 0;
            foreach (var key in _inventory.Keys)
            {
                if (count++ == index) return key;
            }
            return null;
        }

        public bool Contains(Item item)
        {
            return _inventory.ContainsKey(item);
        }

        public bool Contains(Item item, int amount)
        {
            if (_inventory.ContainsKey(item))
            {
                return _inventory[item] >= amount;
            }
            return false;
        }

        public int GetCount(Item item)
        {
            if (_inventory.ContainsKey(item))
            {
                return _inventory[item];
            }
            return 0;
        }
    }

    // Currently unused, could be used for changing from a dictionary back to an array of InventorySlots.
    /// <summary>
    /// A slot in the inventory that holds an item and how much of the item there are.
    /// </summary>
    public class InventorySlot
    {
        public Item item { get; private set; } = null;
        public int count { get; private set; } = 0;

        public bool IsEmpty { get => item == null && count == 0; }

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
        /// Subtracts as much of the amount as possible from the slot. Returns the amount left to subtract from another slot
        /// if the amount to remove is greater than what is stored in the slot.
        /// If count reaches 0, clears the slot.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public int Subtract(int amount)
        {
            if (amount >= count)
            {
                int amountLeftToSubtract = amount - count;
                Clear();
                return amountLeftToSubtract;
            }

            count -= amount;
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