using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items
{
    public class Inventory : IInventory
    {
        private Dictionary<int, List<InventorySlot>> _contentsIndex;        // References each stack holding the itemId.
        private List<InventorySlot> _emptySlots;                            // List of empty inventory slots.
        private InventorySlot[] _inventory;                                 // Inventory list. InventorySlots hold Items and amounts.

        private int _size = 0;

        public int Size { get => _inventory.Length; }                       // Total size of the inventory. How many slots there are.
        // TODO: Track how many empty slots are left.
        public int EmptySlots => _emptySlots.Count;
        public int Stacks => Size - EmptySlots;
        public bool IsFull => EmptySlots <= 0;

        public event IInventory.InventoryDelegate InventoryChangedEvent;

        /// <summary>
        /// Creates a new inventory.
        /// </summary>
        /// <param name="size"></param>
        public Inventory (int size)
        {
            _size = size;
            InitInventory();
        }

        /// <summary>
        /// Initializes the inventory array and index.
        /// </summary>
        public void InitInventory()
        {
            _contentsIndex = new Dictionary<int, List<InventorySlot>>();
            _emptySlots = new List<InventorySlot>();
            _inventory = new InventorySlot[_size];

            for (int i = 0; i < _size; i++)
            {
                InventorySlot slot = new InventorySlot(this, i);
                _inventory[i] = slot;
                _emptySlots.Add(slot);
            }

            InventoryChangedEvent?.Invoke();
        }

        /// <summary>
        /// Gets the next empty slot.
        /// Returns null if none available.
        /// </summary>
        /// <returns></returns>
        private InventorySlot GetEmptySlot()
        {
            if (_emptySlots == null || _emptySlots.Count <= 0) return null;

            InventorySlot slot = _emptySlots[0];
            _emptySlots.Remove(slot);
            return slot;
        }

        /// <summary>
        /// Returns an empty slot to the empty slot list and sorts the list.
        /// </summary>
        private void ReturnEmptySlot(InventorySlot slot)
        {
            slot.Clear();
            _emptySlots.Add(slot);
            SortEmptySlots();
        }

        /// <summary>
        /// Sorts the list of empty slots to be in order according to inventory index.
        /// </summary>
        private void SortEmptySlots()
        {
            _emptySlots.Sort((x, y) => { return (x.InventoryIndex > y.InventoryIndex ? 1 : -1); }); // Sort by the index number.
        }

        /// <summary>
        /// Add an item to the inventory. Returns the amount added.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public int Add(Item item, int amount)
        {
            int addedAmount = 0;

            if (!item.IsStackable && EmptySlots > 0)
            {
                InventorySlot slot = GetEmptySlot();
                if (slot == null)
                {
                    Debug.Log($"Cannot find an empty slot.");
                    return 0;
                }
                _contentsIndex.Add(item.Id, new List<InventorySlot>());
                addedAmount = AddFreshSlot(slot, item, 1);
            }

            if (item.IsStackable)
            {
                // Check if it's already in the list, if so add to those stacks.

                // If finished adding to it's other stacks and there is still more, check if there is more room to add new stacks.
                while (EmptySlots > 0 && amount > 0)
                {
                    InventorySlot slot = GetEmptySlot();
                    if (slot == null)
                    {
                        Debug.Log($"Cannot find an empty slot.");
                        return 0;
                    }
                    _contentsIndex.Add(item.Id, new List<InventorySlot>());
                    addedAmount = AddFreshSlot(slot, item, amount);
                }
            }

            InventoryChangedEvent?.Invoke();
            return 0;


            //if (!item.IsStackable)
            //{
            //    // Not stackable, create a new stack of size 1.
            //    InventorySlot slot = GetEmptySlot();
            //    if (slot == null) return 0; // Check for null slot.
            //    _contentsIndex.Add(item.Id, new List<InventorySlot>());
            //    addedAmount = AddFreshSlot(slot, item, 1);
            //}
            //else
            //{
            //    // If not in the index, create a new entry.
            //    if (!_contentsIndex.ContainsKey(item.Id))
            //    {
            //        Debug.Log($"Adding {item.Name} to index.");
            //        _contentsIndex.Add(item.Id, new List<InventorySlot>());
            //    }

            //    // Try to add to existing entries, if not any then create a new one.
            //    var stacks = _contentsIndex[item.Id];

            //    // Look through each stack in the entry. Add to any existing not full stacks.
            //    for (int i = 0; i < stacks.Count; i++)
            //    {
            //        amount -= stacks[i].Add(item, amount);
            //        Debug.Log($"{item.Name} stack {i} now has {stacks[i].count}.");
            //        if (amount <= 0) break;
            //    }

            //    // If there is still some amount left, create new stacks to fill it out.
            //    while (amount > 0 && EmptySlots > 0)
            //    {
            //        // Add a new stack.
            //        InventorySlot slot = GetEmptySlot();

            //        if (slot == null) return 0; // Check for null slot.

            //        amount -= AddFreshSlot(slot, item, 1);      // Count down how much was actually added to the stack.
            //    }
            //    addedAmount = amount;
            //}
        }

        /// <summary>
        /// Shorthand for adding an item to the correct list and sorting the empty slots.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        private int AddFreshSlot(InventorySlot slot, Item item, int amount)
        {
            int amt = slot.Add(item, amount);
            _contentsIndex[item.Id].Add(slot);
            //_contentsIndex[item.Id].Sort((x, y) => { return (x.InventoryIndex > y.InventoryIndex ? 1 : -1); }); // Sort by the index number.
            //SortEmptySlots();
            return amt;
        }

        /// <summary>
        /// Subtracts the item from the list, return true if anything was removed.
        /// // TODO: Make it only remove a specific item and not use Item.Id.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Subtract(Item item, int amount)
        {
            return Subtract(item.Id, amount);
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

            if (_contentsIndex.ContainsKey(itemId))
            {
                var stacks = _contentsIndex[itemId];
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
                        ReturnEmptySlot(slot); ;
                    }
                }
                // Remove item from index if empty.
                if (stacks.Count <= 0)
                {
                    Remove(itemId);
                }

                InventoryChangedEvent?.Invoke();
                return true;
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
            return Remove(item.Id);
        }

        /// <summary>
        /// Removes a given item from the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(ItemObject item)
        {
            return Remove(item.Id);
        }

        /// <summary>
        /// Removes the item with the given id from the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool Remove(int itemId)
        {
            if (Contains(itemId))
            {
                // Clear each InventorySlot
                foreach (var slot in _contentsIndex[itemId])
                {
                    ReturnEmptySlot(slot);
                }

                // Remove the index reference.
                _contentsIndex.Remove(itemId);

                InventoryChangedEvent?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the inventory slot at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public InventorySlot Get(int index)
        {
            if (index < 0 || index >= Size) return null;

            return _inventory[index];
        }

        public Item GetItem(int index)
        {
            if (index < 0 || index >= Size) return null;

            return _inventory[index].item;
        }

        /// <summary>
        /// Finds the first index of the given item in the inventory or -1 if
        /// it is not there.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetIndex(Item item)
        {
            if (_contentsIndex.ContainsKey(item.Id))
            {
                return _contentsIndex[item.Id][0].InventoryIndex;
            }
            return -1;
        }

        /// <summary>
        /// Checks if an instance of the item is in the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool Contains(int itemId)
        {
            return _contentsIndex.ContainsKey(itemId);
        }

        /// <summary>
        /// Checks if there are enough instances of the given item in the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ContainsAmount(int itemId, int amount)
        {
            if (_contentsIndex.ContainsKey(itemId))
            {
                int count = 0;
                foreach (var stack in _contentsIndex[itemId])
                {
                    count += stack.count;
                }
                return count >= amount;
            }
            return false;
        }

        /// <summary>
        /// Gets the count of a specific item with the given id.
        /// There is no version for Items because there should not checking for multiples
        /// of a specific instance.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int GetCount(ItemObject itemId)
        {
            return GetCount(itemId.Id);
        }

        /// <summary>
        /// Gets the count of a specific item with the given id.
        /// There is no version for Items because there should not checking for multiples
        /// of a specific instance.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int GetCount(int itemId)
        {
            if (_contentsIndex.ContainsKey(itemId))
            {
                int count = 0;
                Debug.Log($"itemId: {itemId} has {_contentsIndex[itemId].Count} stacks.");
                foreach (var stack in _contentsIndex[itemId])
                {
                    Debug.Log($"stack count: {stack.count}. Count = {count}.");
                    count += stack.count;
                }
                return count;
            }
            return 0;
        }

        /// <summary>
        /// Converts the item index into a string.
        /// </summary>
        /// <returns></returns>
        public string IndexToString()
        {
            string str = "{\n";

            foreach (var entry in _contentsIndex)
            {
                if (entry.Value == null) continue;

                str += "\t" + GameManager.instance.ItemList.ItemList[entry.Key].Name + " (" + entry.Key + "): ";
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
            for (int i = 1; i < Size; i++)
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

    /// <summary>
    /// A slot in the inventory that holds an item and how much of the item there are.
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        public int InventoryIndex { get; private set; }
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

        public InventorySlot(Inventory _parentInventory, int index)
        {
            InventoryIndex = index;
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