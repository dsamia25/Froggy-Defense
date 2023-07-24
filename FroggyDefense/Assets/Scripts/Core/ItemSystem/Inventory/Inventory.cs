using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items
{
    public class Inventory : MonoBehaviour, IInventory
    {
        private Dictionary<Item, List<InventorySlot>> _contentsIndex;               // A dictionary to find items in the list.
        [SerializeField] private List<InventorySlot> _inventory;                    // A list of everything in the inventory.

        public int Size { get => _inventory.Count; }
        public int MaxSize = 32;                                                    // Hard coded mx size for now.

        public event IInventory.InventoryDelegate InventoryChangedEvent;

        private void Awake()
        {
            // TODO: Make a way to load a saved inventory instead of creating a new one.
            // Instantiate the inventory.
            InitInventory();
        }

        public void InitInventory()
        {
            _contentsIndex = new Dictionary<Item, List<InventorySlot>>();
            _inventory = new List<InventorySlot>();
            InventoryChangedEvent?.Invoke();
        }

        /// <summary>
        /// Add an item to the inventory. Returns the amount added.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void Add(Item item, int amount)
        {
            try
            {
                // If stackable, look if there is already a stack in the contents index.
                if (item.IsStackable)
                {
                    Debug.Log($"Adding {item.Name} is stackable. Max Stack Size: {item.StackSize}.");

                    // If not in the index, create a new entry.
                    if (!_contentsIndex.ContainsKey(item))
                    {
                        Debug.Log($"Adding {item.Name} to index.");
                        _contentsIndex.Add(item, new List<InventorySlot>());
                    }

                    // Try to add to existing entries, if not any then create a new one.
                    var stacks = _contentsIndex[item];

                    // Look through each stack in the entry. Add to any existing not full stacks.
                    for (int i = 0; i < stacks.Count; i++)
                    {
                        amount -= stacks[i].Add(item, amount);
                        Debug.Log($"{item.Name} stack {i} now has {stacks[i].count}.");
                        if (amount <= 0) break;
                    }

                    Debug.Log($"Out of empty stacks for {item.Name}.");
                    // If there is still some amount left, create new stacks to fill it out.
                    while (amount > 0 && Size < MaxSize)
                    {
                        // Add a new stack.
                        InventorySlot slot = new InventorySlot(this);
                        slot.Add(item, amount);
                        _inventory.Add(slot);
                        _contentsIndex[item].Add(slot);
                        amount -= slot.count;      // Count down how much was actually added to the stack.
                    }
                }
                else                 // else, just add it to the inventory
                {
                    Debug.Log($"Adding not stackable item {item.Name}");
                    // Not stackable, create a new stack of size 1.
                    InventorySlot slot = new InventorySlot(this);
                    slot.Add(item, 1);

                    _inventory.Add(slot);
                    Debug.Log($"Inventory size: {Size}.");
                    _contentsIndex.Add(item, new List<InventorySlot>());
                    Debug.Log($"Index size: {_contentsIndex.Count}.");
                    _contentsIndex[item].Add(slot);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Error adding item to inventory: {e}");
            }

            PrintIndex();
            InventoryChangedEvent?.Invoke();
        }

        /// <summary>
        /// Subtracts the item from the list, return true if anything was removed.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Subtract(Item item, int amount)
        {
            if (amount <= 0) return false;

            if (_contentsIndex.ContainsKey(item))
            {
                var stacks = _contentsIndex[item];
                // Start subtracting from the last slot first.
                for (int i = stacks.Count - 1; i >= 0; i--)
                {
                    if (amount <= 0) break;
                    
                    InventorySlot slot = stacks[i];
                    amount = slot.Subtract(amount);

                    // Remove empty stacks.
                    if (slot.IsEmpty)
                    {
                        stacks.Remove(slot);
                        _inventory.Remove(slot);
                    }
                }
                // Remove item from index if empty.
                if (stacks.Count <= 0)
                {
                    Remove(item);
                }

                InventoryChangedEvent?.Invoke();
                return true;
            }
            InventoryChangedEvent?.Invoke();
            return false;
        }

        /// <summary>
        /// Subtracts a given amount of the item from the inventory. Uses an item id.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Subtract(int itemId, int amount)
        {
            if (amount <= 0) return false;

            foreach (InventorySlot slot in _inventory)
            {
                if (slot.item.Id == itemId)
                {

                }
            }

            InventoryChangedEvent?.Invoke();
            return false;
        }

        /// <summary>
        /// Removes an item from the list and contents index.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(Item item)
        {
            if (Contains(item))
            {
                Debug.Log($"Removing ({item.Name}) from inventory.");

                // Remove each InventorySlot
                foreach (var stack in _contentsIndex[item])
                {
                    _inventory.Remove(stack);
                }

                // Remove the index reference.
                _contentsIndex.Remove(item);

                InventoryChangedEvent?.Invoke();
                return true;
            }
            return false;
        }

        public bool Remove(int itemId)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Finds the index of the given item in the inventory or -1 if
        /// it is not there.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetIndex(Item item)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Contains(Item item)
        {
            return _contentsIndex.ContainsKey(item);
        }

        public bool Contains(int itemId)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Item item, int amount)
        {
            if (_contentsIndex.ContainsKey(item))
            {
                int count = 0;
                foreach (var stack in _contentsIndex[item])
                {
                    count += stack.count;
                }
                return count >= amount;
            }
            return false;
        }

        public bool Contains(int itemId, int amount)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Item item)
        {
            if (_contentsIndex.ContainsKey(item))
            {
                int count = 0;
                foreach (var stack in _contentsIndex[item])
                {
                    count += stack.count;
                }
                return count;
            }
            return 0;
        }

        public int GetCount(int itemId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the inventory contains the input id based on the item id.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        //public int GetCount(ItemObject item)
        //{
        //    foreach (InventorySlot slot in _inventory)
        //    {
        //        if (slot.item.Id == item.Id)
        //        {
        //            return slot.count;
        //        }
        //    }
        //    return 0;
        //}

        public string IndexToString()
        {
            string str = "{\n";

            foreach (var entry in _contentsIndex)
            {
                str += "\t" + entry.Key.Name + " (" + entry.Key.Id + "): ";
                foreach (var stack in entry.Value)
                {
                    str += stack.count + ", ";
                }
                str += "\n";
            }
            str += "}";

            return str;
        }

        public void PrintIndex()
        {
            Debug.Log(IndexToString());
        }

        public override string ToString()
        {
            string str = "{\n";

            InventorySlot slot = _inventory[0];
            str += "\t0: " + slot.count + " - " + slot.item.Name;
            for (int i = 1; i < _inventory.Count; i++)
            {
                slot = _inventory[i];
                str += ",\n\t" + i + ": " + slot.count + " - " + slot.item.Name;
            }
            str += "\n}";

            return str;
        }

        public void Print()
        {
            Debug.Log(ToString());
        }
    }

    // Currently unused, could be used for changing from a dictionary back to an array of InventorySlots.
    /// <summary>
    /// A slot in the inventory that holds an item and how much of the item there are.
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        public Item item { get; private set; } = null;
        public int count { get; private set; } = 0;
        public int stackSize
        {
            get {
                if (item == null) return 0;
                if (item.IsStackable) return ItemObject.StackSize;
                return 1;
            }
        }
        public Inventory parentInventory;

        public bool IsEmpty { get => item == null && count == 0; }

        public InventorySlot(Inventory _parentInventory)
        {
            item = null;
            count = 0;
            parentInventory = _parentInventory;
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
            }

            if (!item.Equals(_item))
            {
                return 0;
            }

            if (count >= stackSize)
            {
                return 0;
            }

            if (count + amount >= stackSize)
            {
                amount = stackSize - count;
                count = stackSize;
            } else
            {
                count += amount;
            }
            return amount;
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
            Debug.Log("Clearing (" + item.Name + ") inventory slot.");
            item = null;
            count = 0;
        }

        /// <summary>
        /// Tries to use the item in the slot.
        /// </summary>
        public void UseItem()
        {
            if (item.Use())
            {
                parentInventory.Subtract(item, item.CountSubtractPerUse);
            }
        }
    }
}