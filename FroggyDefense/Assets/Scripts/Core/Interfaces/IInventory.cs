using FroggyDefense.Core.Items;

namespace FroggyDefense.Core
{
    /// <summary>
    /// Defines key methods needed for an inventory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInventory
    {
        public int Size { get; }
        public delegate void InventoryDelegate();
        public event InventoryDelegate InventoryChangedEvent;

        /// <summary>
        /// Adds a set amount of new items to the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void Add(Item item, int amount);

        /// <summary>
        /// Subtracts a given amount of the item from the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Subtract(Item item, int amount);

        /// <summary>
        /// Subtracts the item with the given id from the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Subtract(int itemId, int amount);

        /// <summary>
        /// Removes a given item from the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(Item item);

        /// <summary>
        /// Removes the item with the given id from the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool Remove(int itemId);

        /// <summary>
        /// Checks if a certain item is in the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Item item);

        /// <summary>
        /// Checks if an item with the input id is in the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool Contains(int itemId);

        /// <summary>
        /// Checks if there is a certain amount of the given item in the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Item item, int amount);

        /// <summary>
        /// Checks if there is a certain amount of a given item with the input id in the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Contains(int itemId, int amount);

        /// <summary>
        /// Gets the count of a specific item.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetCount(Item item);

        /// <summary>
        /// Gets the count of a specific item with the given id.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int GetCount(int itemId);

        /// <summary>
        /// Gets the count of a specific item.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        //public int GetCount(ItemObject item);

        /// <summary>
        /// Returns the contents of the inventory as a string.
        /// </summary>
        /// <returns></returns>
        public string ToString();

        /// <summary>
        /// Prints out the contents of the inventory to the console.
        /// </summary>
        public void Print();
    }
}