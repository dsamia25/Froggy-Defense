using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items
{
    public class FixedInventory: IInventory
    {
        private FixedInventorySlot[] inventory;
        private List<FixedInventorySlot> emptySlots;
        private Dictionary<int, List<FixedInventorySlot>> itemIndex;

        public event IInventory.InventoryDelegate InventoryChangedEvent;

        public int Size { get; protected set; }

        /// <summary>
        /// Initialize a new inventory.
        /// </summary>
        public FixedInventory(int size)
        {
            Size = size;
            inventory = new FixedInventorySlot[Size];
            emptySlots = new List<FixedInventorySlot>();
            itemIndex = new Dictionary<int, List<FixedInventorySlot>>();

            for (int i = 0; i < Size; i++)
            {
                FixedInventorySlot slot = new FixedInventorySlot(this, i);
                inventory[i] = slot;
                emptySlots.Add(slot);
            }
        }

        /// <summary>
        /// Gets the inventory slot at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FixedInventorySlot Get(int index)
        {
            if (index < 0 || index >= Size) return null;

            return inventory[index];
        }

        /// <summary>
        /// Gets the next empty inventory slot.
        /// </summary>
        /// <returns></returns>
        private FixedInventorySlot GetEmptySlot()
        {
            FixedInventorySlot slot = emptySlots[0];
            emptySlots.Remove(slot);
            return slot;
        }

        private FixedInventorySlot GetSlot(Item item)
        {
            FixedInventorySlot slot;

            // Check for existing stacks.
            if (item.IsStackable && itemIndex.ContainsKey(item.Id))
            {
                for (int i = 0; i < itemIndex[item.Id].Count; i++)
                {
                    slot = itemIndex[item.Id][i];
                    if (slot.HasRoom)
                    {
                        return slot;
                    }
                }
            }

            // Check if no more empty stacks.
            if (emptySlots.Count <= 0)
            {
                return null;
            }

            // Get new stack
            slot = emptySlots[0];
            emptySlots.Remove(slot);
            return slot;
        }

        /// <summary>
        /// Adds the item to the inventory.
        /// Tries to add the full amount.
        /// Returns how many were actually added.
        /// </summary>
        /// <returns></returns>
        public int Add(Item item, int amount)
        {
            int amountAdded = 0;
            FixedInventorySlot slot;
            while (amount > amountAdded && ((slot = GetSlot(item)) != null))
            {
                if (!itemIndex.ContainsKey(item.Id)) itemIndex.Add(item.Id, new List<FixedInventorySlot>());
                amountAdded += slot.Add(item, amount - amountAdded);
                itemIndex[item.Id].Add(slot);
                Debug.Log("ItemIndex: " + IndexToString());
            }
            return amountAdded;
        }

        public bool Subtract(int itemId, int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(int itemId)
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(int itemId)
        {
            return itemIndex.ContainsKey(itemId);
        }

        public bool ContainsAmount(int itemId, int amount)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Counts how many items with the given id are in the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int GetCount(int itemId)
        {
            if (!itemIndex.ContainsKey(itemId)) return 0;

            int count = 0;
            for (int i = 0; i < itemIndex[itemId].Count; i++)
            {
                count += itemIndex[itemId][i].count;
            }
            return count;
        }

        /// <summary>
        /// Gets the amount of stacks of an item.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int GetStacks(int itemId)
        {
            return itemIndex.ContainsKey(itemId) ? itemIndex[itemId].Count : 0;
        }

        public void Print()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Converts the item index into a string.
        /// </summary>
        /// <returns></returns>
        public string IndexToString()
        {
            string str = "{\n";

            foreach (var entry in itemIndex)
            {
                if (entry.Value == null) continue;

                str += "\t(" + entry.Key + "): ";
                foreach (var stack in entry.Value)
                {
                    str += stack.count + ", ";
                }
                str += "\n";
            }
            str += "}";

            return str;
        }
    }

    /// <summary>
    /// A slot in the inventory that holds an item and how much of the item there are.
    /// </summary>
    [System.Serializable]
    public class FixedInventorySlot
    {
        public int InventoryIndex { get; private set; }
        public Item item { get; private set; } = null;
        public int count { get; private set; } = 0;
        public int StackSize
        {
            get
            {
                if (item == null) return 0;
                if (item.IsStackable) return ItemObject.StackSize;
                return 1;
            }
        }
        public FixedInventory parentInventory;

        public bool IsEmpty => item == null && count == 0;
        public bool HasRoom => item != null && count < StackSize;
        public bool IsFull => item != null && count >= StackSize;

        public FixedInventorySlot(FixedInventory _parentInventory, int index)
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
            // If different item
            if (item != null && !item.Equals(_item))
            {
                return 0;
            }

            // If whole new item
            if (item == null)
            {
                item = _item;
            }

            // If already full
            if (count >= StackSize)
            {
                return 0;
            }

            if (count + amount >= StackSize)
            {
                amount = StackSize - count;
                count = StackSize;
                if (!item.IsStackable)
                {
                    return 1;
                }
            }
            else
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
                //parentInventory.Subtract(item, item.CountSubtractPerUse);
            }
        }
    }
}