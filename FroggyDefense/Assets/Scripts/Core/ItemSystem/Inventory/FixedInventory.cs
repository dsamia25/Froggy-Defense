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

        /// <summary>
        /// Gets an item slot to add to.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private FixedInventorySlot GetSlot(Item item)
        {
            if (item == null) return null;

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

            // Register the new stack.
            if (!itemIndex.ContainsKey(item.Id)) itemIndex.Add(item.Id, new List<FixedInventorySlot>());
            itemIndex[item.Id].Add(slot);

            return slot;
        }

        /// <summary>
        /// Returns the slot to the empty pool and removes it from indexes.
        /// </summary>
        /// <param name="slot"></param>
        private void ReturnSlot(int itemId, FixedInventorySlot slot)
        {
            slot.Clear();
            emptySlots.Add(slot);

            itemIndex[itemId].Remove(slot);
            if (itemIndex[itemId].Count <= 0)
            {
                itemIndex.Remove(itemId);
            }
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
                amountAdded += slot.Add(item, amount - amountAdded);
            }
            Debug.Log($"ItemIndex: (+{amount}) {IndexToString()}");
            InventoryChangedEvent?.Invoke();
            return amountAdded;
        }

        /// <summary>
        /// Tries to subtract the input amount of the given item from the inventory.
        /// Returns true if ANYTHING WAS REMOVED, even if it was less tha nthe input amount.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Subtract(int itemId, int amount)
        {
            if (!itemIndex.ContainsKey(itemId)) return false;

            int amountLeftToSubtract = amount;
            for (int i = itemIndex[itemId].Count - 1; i >= 0; i--)
            {
                FixedInventorySlot slot = itemIndex[itemId][i];
                amountLeftToSubtract = slot.Subtract(amountLeftToSubtract);
                if (slot.IsEmpty) ReturnSlot(itemId, slot);
                if (amountLeftToSubtract <= 0) break;
            }
            Debug.Log($"ItemIndex: (-{amount}) {IndexToString()}");
            InventoryChangedEvent?.Invoke();
            return true;
        }

        /// <summary>
        /// Clears the input item from the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool Remove(int itemId)
        {
            if (!itemIndex.ContainsKey(itemId)) return false;

            for (int i = itemIndex[itemId].Count - 1; i >= 0; i--)
            {
                ReturnSlot(itemId, itemIndex[itemId][i]);
            }
            InventoryChangedEvent?.Invoke();
            return true;
        }

        /// <summary>
        /// Checks if there are any of the item in the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool Contains(int itemId)
        {
            return itemIndex.ContainsKey(itemId);
        }

        /// <summary>
        /// Checks if the inventory has at least this amount of the item.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ContainsAmount(int itemId, int amount)
        {
            return GetCount(itemId) >= amount;
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

        /// <summary>
        /// Swaps the items in the InventorySlots at the two positions.
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        public void Swap(int posA, int posB)
        {
            if (posA < 0 || posA >= Size || posB < 0 || posB >= Size) return;

            FixedInventorySlot.Swap(inventory[posA], inventory[posB]);
        }

        /// <summary>
        /// Converts the inventory to a string format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "{\n";

            str += $"\t{inventory[0].item.Id}";
            for (int i = 1; i < inventory.Length; i++)
            {
                str += $"\t, {inventory[i].item.Id}";
            }
            str += "}";

            return str;
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
        /// Swaps the contents of the two slots.
        /// </summary>
        /// <param name="slotA"></param>
        /// <param name="slotB"></param>
        public static void Swap(FixedInventorySlot slotA, FixedInventorySlot slotB)
        {
            Item tempItem = slotA.item;
            int tempCount = slotA.count;

            slotA.item = slotB.item;
            slotA.count = slotB.count;
            slotB.item = tempItem;
            slotB.count = tempCount;
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
                parentInventory.Subtract(item.Id, item.CountSubtractPerUse);
                //Debug.Log("Using");
            }
        }
    }
}