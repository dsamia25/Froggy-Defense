using UnityEngine;
using FroggyDefense.Core;
using System.Collections.Generic;

namespace FroggyDefense.Items
{
    public class Inventory : MonoBehaviour, IInventory
    {
        private Dictionary<Item, InventorySlot> _itemLookupChart;           // A dictionary to find items in the list.
        private List<InventorySlot> _inventory;                             // A list of everything in the inventory.

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
            _itemLookupChart = new Dictionary<Item, InventorySlot>();
            _inventory = new List<InventorySlot>();
            InventoryChangedEvent?.Invoke();
        }

        public void Add(Item item, int amount)
        {
            try
            {
                if (item.IsStackable)
                {
                    // If the item is stackable, look for it in the dictionary.
                    if (_itemLookupChart.ContainsKey(item))
                    {
                        // If the item is already in the dictionary, add to the existing stack.
                        _itemLookupChart[item].Add(item, amount);
                    }
                    else
                    {
                        // If the item is not already in the dictionary, create a new entry.
                        var slot = new InventorySlot();
                        slot.Add(item, amount);
                        _inventory.Add(slot);
                        _itemLookupChart.Add(item, slot);
                    }
                }
                else
                {
                    // If the item is not stackable, it will always need a new entry with a size of 1.
                    var slot = new InventorySlot();
                    slot.Add(item, 1);
                    _inventory.Add(slot);
                    _itemLookupChart.Add(item, slot);
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

            if (_itemLookupChart.ContainsKey(item))
            {
                _itemLookupChart[item].Subtract(amount);

                if (_itemLookupChart[item].count <= 0)
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
            if (Contains(item))
            {
                _inventory.Remove(_itemLookupChart[item]);
                _itemLookupChart.Remove(item);
                InventoryChangedEvent?.Invoke();
                return true;
            }
            return false;
        }

        public InventorySlot Get(int index)
        {
            if (index < 0 || index >= _inventory.Count) return null;

            return _inventory[index];
        }

        public Item GetItem(int index)
        {
            if (index < 0 || index >= _inventory.Count) return null;

            return _inventory[index].item;
        }

        public bool Contains(Item item)
        {
            return _itemLookupChart.ContainsKey(item);
        }

        public bool Contains(Item item, int amount)
        {
            if (_itemLookupChart.ContainsKey(item))
            {
                return _itemLookupChart[item].count >= amount;
            }
            return false;
        }

        public int GetCount(Item item)
        {
            if (_itemLookupChart.ContainsKey(item))
            {
                return _itemLookupChart[item].count;
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
        public int Add(Item _item, int amount)
        {
            if (item == null)
            {
                item = _item;
                count = amount;
                return amount;
            } else if (item == _item)
            {
                count += amount;
                return amount;
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

        /// <summary>
        /// Tries to use the item in the slot.
        /// </summary>
        public void UseItem()
        {
            item.Use();
            Subtract(item.CountSubtractPerUse);
        }
    }
}