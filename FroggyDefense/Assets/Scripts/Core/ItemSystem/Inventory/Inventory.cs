using UnityEngine;
using System.Collections.Generic;

namespace FroggyDefense.Core.Items
{
    public class Inventory : MonoBehaviour, IInventory
    {
        private Dictionary<Item, InventorySlot> _contentsIndex;             // A dictionary to find items in the list.
        [SerializeField] private List<InventorySlot> _inventory;            // A list of everything in the inventory.
        [SerializeField] private List<InventorySlot> _emptySlots;           // A list of all the empty slots on the board.

        public int Size { get => _inventory.Count; }

        public event IInventory.InventoryDelegate InventoryChangedEvent;

        private void Awake()
        {
            // TODO: Make a way to load a saved inventory instead of creating a new one.
            // Instantiate the inventory.
            InitInventory();

            //Debug.LogWarning("Testing list order:");

            //List<string> testList = new List<string>();
            //testList.Add("a");
            //testList.Add("b");
            //testList.Add("c");
            //Debug.LogWarning("First pass: [" + (0 < testList.Count ? testList[0] : "NULL") + ", " + (1 < testList.Count ? testList[1] : "NULL") + ", " + (2 < testList.Count ? testList[2] : "NULL") + ", " + (3 < testList.Count ? testList[3] : "NULL") + "].");

            //testList.Remove("a");
            //Debug.LogWarning("Removed index 0 (\'a\'): [" + (0 < testList.Count ? testList[0] : "NULL") + ", " + (1 < testList.Count ? testList[1] : "NULL") + ", " + (2 < testList.Count ? testList[2] : "NULL") + ", " + (3 < testList.Count ? testList[3] : "NULL") + "].");

            //testList.Add("a");
            //Debug.LogWarning("Readded (\'a\'): [" + (0 < testList.Count ? testList[0] : "NULL") + ", " + (1 < testList.Count ? testList[1] : "NULL") + ", " + (2 < testList.Count ? testList[2] : "NULL") + ", " + (3 < testList.Count ? testList[3] : "NULL") + "].");

            //Debug.LogWarning("Ending test.");
        }

        public void InitInventory()
        {
            _contentsIndex = new Dictionary<Item, InventorySlot>();
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
                    if (_contentsIndex.ContainsKey(item))
                    {
                        // If the item is already in the dictionary, add to the existing stack.
                        _contentsIndex[item].Add(item, amount);
                    }
                    else
                    {
                        // If the item is not already in the dictionary, create a new entry.
                        var slot = new InventorySlot(this);
                        slot.Add(item, amount);
                        _inventory.Add(slot);
                        _contentsIndex.Add(item, slot);
                    }
                }
                else
                {
                    // If the item is not stackable, it will always need a new entry with a size of 1.
                    var slot = new InventorySlot(this);
                    slot.Add(item, 1);
                    _inventory.Add(slot);
                    _contentsIndex.Add(item, slot);
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

            if (_contentsIndex.ContainsKey(item))
            {
                var slot = _contentsIndex[item];
                slot.Subtract(amount);
                if (slot.IsEmpty)
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
                Debug.Log("Removing (" + item.Name + ") from inventory.");
                _inventory.Remove(_contentsIndex[item]);
                _contentsIndex.Remove(item);
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
            return _contentsIndex.ContainsKey(item);
        }

        public bool Contains(Item item, int amount)
        {
            if (_contentsIndex.ContainsKey(item))
            {
                return _contentsIndex[item].count >= amount;
            }
            return false;
        }

        public int GetCount(Item item)
        {
            if (_contentsIndex.ContainsKey(item))
            {
                return _contentsIndex[item].count;
            }
            return 0;
        }

        /// <summary>
        /// Checks if the inventory contains the input id based on the item id.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetCount(ItemObject item)
        {
            foreach (InventorySlot slot in _inventory)
            {
                if (slot.item.Id == item.Id)
                {
                    return slot.count;
                }
            }
            return 0;
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
            Debug.Log("Clearing (" + item.Name + ") inventory slot.");
            item = null;
            count = 0;
        }

        /// <summary>
        /// Tries to use the item in the slot.
        /// </summary>
        public void UseItem()
        {
            item.Use();
            parentInventory.Subtract(item, item.CountSubtractPerUse);
        }
    }
}